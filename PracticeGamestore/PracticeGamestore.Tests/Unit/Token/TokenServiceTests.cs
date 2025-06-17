using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Options;
using PracticeGamestore.Business.Services.Token;

namespace PracticeGamestore.Tests.Unit.Token;

[TestFixture]
public class TokenServiceTests
{
    private Mock<IOptions<JwtOptions>> _jwtOptionsMock;
    private Mock<IConfiguration> _configurationMock;
    private TokenService _tokenService;
    private JwtOptions _testJwtOptions;

    [SetUp]
    public void SetUp()
    {
        _testJwtOptions = new JwtOptions
        {
            SecretKey = "this-is-a-very-secure-key-for-testing-purposes-that-is-long-enough-123456789",
            Issuer = "https://api.test-gamestore.com",
            Audience = "https://test-gamestore.com",
            ExpirationTimeInMinutes = 60
        };

        _jwtOptionsMock = new Mock<IOptions<JwtOptions>>();
        _jwtOptionsMock.Setup(x => x.Value).Returns(_testJwtOptions);

        _configurationMock = new Mock<IConfiguration>();
        
        _tokenService = new TokenService(_jwtOptionsMock.Object, _configurationMock.Object);
    }
    
    [Test]
    public void CreateTokenValidationParameters_WhenJwtOptionsAreValid_ShouldReturnCorrectParametersAndSetCorrectSigningKey()
    {
        // Act
        var validationParameters = TokenService.CreateTokenValidationParameters(_testJwtOptions);

        // Assert
        Assert.That(validationParameters, Is.Not.Null);
        
        Assert.That(validationParameters.ValidateIssuer, Is.True);
        Assert.That(validationParameters.ValidIssuer, Is.EqualTo(_testJwtOptions.Issuer));

        Assert.That(validationParameters.ValidateAudience, Is.True);
        Assert.That(validationParameters.ValidAudience, Is.EqualTo(_testJwtOptions.Audience));
        
        Assert.That(validationParameters.ClockSkew, Is.EqualTo(TimeSpan.Zero));
        Assert.That(validationParameters.ValidateLifetime, Is.True);

        Assert.That(validationParameters.ValidateIssuerSigningKey, Is.True);
        Assert.That(validationParameters.IssuerSigningKey, Is.Not.Null);
        Assert.That(validationParameters.IssuerSigningKey, Is.InstanceOf<SymmetricSecurityKey>());
        var symmetricKey = validationParameters.IssuerSigningKey as SymmetricSecurityKey;
        var expectedKey = Encoding.UTF8.GetBytes(_testJwtOptions.SecretKey);
        Assert.That(symmetricKey!.Key, Is.EqualTo(expectedKey));
    }

    [Test]
    public void GenerateJwtToken_WhenUserIsValid_ShouldReturnValidTokenResponseDto()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        
        // Act
        var result = _tokenService.GenerateJwtToken(user);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(user.Id));
        Assert.That(result.Token, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Empty);
        Assert.That(result.Expiration, Is.EqualTo(_testJwtOptions.ExpirationTimeInMinutes));
        Assert.That(result.Token.Split('.').Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateJwtToken_WhenUserIsValid_ShouldContainCorrectClaims()
    {        
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        
        // Act
        var result = _tokenService.GenerateJwtToken(user);
        
        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(result.Token);
        
        var nameIdentifierClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "nameid");
        var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "email");
        var roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "role");
        
        Assert.That(nameIdentifierClaim, Is.Not.Null, "NameIdentifier claim ('nameid') not found");
        Assert.That(nameIdentifierClaim!.Value, Is.EqualTo(user.Id.ToString()));
        
        Assert.That(emailClaim, Is.Not.Null, "Email claim not found");
        Assert.That(emailClaim!.Value, Is.EqualTo(user.Email));
        
        Assert.That(roleClaim, Is.Not.Null, "Role claim not found");
        Assert.That(roleClaim!.Value, Is.EqualTo(user.Role.ToString()));
    }

    [TestCase(DataAccess.Enums.UserRole.Admin, TestName = "Admin role")]
    [TestCase(DataAccess.Enums.UserRole.User, TestName = "User role")]
    [TestCase(DataAccess.Enums.UserRole.Manager, TestName = "Manager role")]
    public void GenerateJwtToken_WhenUserHasDifferentRoles_ShouldContainCorrectRoleClaim(DataAccess.Enums.UserRole role)
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        user.Role = role;
        
        // Act
        var result = _tokenService.GenerateJwtToken(user);
        
        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(result.Token);
        var roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "role");
        
        Assert.That(roleClaim, Is.Not.Null, "Role claim not found");
        Assert.That(roleClaim!.Value, Is.EqualTo(role.ToString()));
    }

    [Test]
    public void GenerateJwtToken_WhenUserIsValid_ShouldHaveCorrectIssuerAndAudience()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        
        // Act
        var result = _tokenService.GenerateJwtToken(user);
        
        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(result.Token);
        
        Assert.That(jsonToken.Issuer, Is.EqualTo(_testJwtOptions.Issuer));
        Assert.That(jsonToken.Audiences.First(), Is.EqualTo(_testJwtOptions.Audience));
    }

    [Test]
    public void GenerateJwtToken_WhenUserIsValid_ShouldHaveCorrectExpiryTime()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        var beforeGeneration = DateTime.UtcNow;
        
        // Act
        var result = _tokenService.GenerateJwtToken(user);
        var afterGeneration = DateTime.UtcNow;
        
        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(result.Token);
        
        var expectedMinExpiry = beforeGeneration.AddMinutes(_testJwtOptions.ExpirationTimeInMinutes);
        var expectedMaxExpiry = afterGeneration.AddMinutes(_testJwtOptions.ExpirationTimeInMinutes);
        
        Assert.That(jsonToken.ValidTo, Is.GreaterThanOrEqualTo(expectedMinExpiry.AddSeconds(-1)));
        Assert.That(jsonToken.ValidTo, Is.LessThanOrEqualTo(expectedMaxExpiry.AddSeconds(1)));
    }

    [Test]
    public void GenerateJwtToken_WhenUserIsValid_ShouldUseCorrectSigningAlgorithm()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        
        // Act
        var result = _tokenService.GenerateJwtToken(user);
        
        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(result.Token);
        
        Assert.That(jsonToken.Header.Alg, Is.EqualTo("HS256"));
    }

    [Test]
    public void GenerateJwtToken_WhenCalledMultipleTimes_ShouldGenerateDifferentTokens()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        
        // Act
        var result1 = _tokenService.GenerateJwtToken(user);
        Thread.Sleep(1000); 
        var result2 = _tokenService.GenerateJwtToken(user);
        
        // Assert
        Assert.That(result1.Token, Is.Not.EqualTo(result2.Token));
        Assert.That(result1.UserId, Is.EqualTo(result2.UserId));
        Assert.That(result1.Expiration, Is.EqualTo(result2.Expiration));
    }
}