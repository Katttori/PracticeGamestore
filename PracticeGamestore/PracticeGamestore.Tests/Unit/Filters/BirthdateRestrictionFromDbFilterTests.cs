using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.Filters;

namespace PracticeGamestore.Tests.Unit.Filters;

[TestFixture]
public class BirthdateRestrictionFromDbFilterTests
{
    private ActionExecutingContext _context;
    private Mock<IUserService> _userService;
    private BirthdateRestrictionFromDbFilter _filter;

    [SetUp]
    public void SetUp()
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        _userService = new Mock<IUserService>();
        _context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object());
        _filter = new BirthdateRestrictionFromDbFilter(_userService.Object);
    }

    [Test]
    public async Task OnActionExecuting_WhenUserIsAdult_ShouldSetUnderageValueToFalseInHttpContext()
    {
        // Arrange
        var id = Guid.NewGuid();
        var adultBirthDate = DateTime.Today.AddYears(-20);
        PrepareUser(id, adultBirthDate);
        
        // Act
        await _filter.OnActionExecutionAsync(_context, DummyNext);
        
        // Assert
        var isAdult = (bool)_context.HttpContext.Items[HttpContextCustomItems.UnderageIndicator]!;
        Assert.That(isAdult, Is.False);
    }
    
    [Test]
    public async Task OnActionExecuting_WhenUserIsNotAdult_ShouldSetUnderageValueToTrueInHttpContext()
    {
        // Arrange
        var id = Guid.NewGuid();
        var underageBirthDate = DateTime.Today.AddYears(-16);
        PrepareUser(id, underageBirthDate);
        
        // Act
        await _filter.OnActionExecutionAsync(_context, DummyNext);
        
        // Assert
        var underage = _context.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] as bool? ?? false;
        Assert.That(underage, Is.True);
    }
    
    [Test]
    public async Task OnActionExecuting_WhenUserNotFound_ShouldSetUnderageValueToTrueInHttpContext()
    {
        // Arrange
        var id = Guid.NewGuid();
        PrepareUser(id, DateTime.Today.AddYears(-20));
        _userService
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((UserDto?)null);
        
        // Act
        await _filter.OnActionExecutionAsync(_context, DummyNext);
        
        // Assert
        var underage = _context.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] as bool? ?? false;
        Assert.That(underage, Is.True);
    }
    
    
    private Task<ActionExecutedContext> DummyNext()
    {
        var executed = new ActionExecutedContext(_context, _context.Filters, _context.Controller);
        return Task.FromResult(executed);
    }
    
    private void PrepareUser(Guid id, DateTime birthDate)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString())
        }, "Test");
        _context.HttpContext.User = new ClaimsPrincipal(identity);
        
        var user = TestData.User.GenerateUserDto(id);
        user.BirthDate = birthDate;
        
        _userService
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(user);
    }
}