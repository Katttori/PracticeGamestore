using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Game;
using PracticeGamestore.Tests.TestData;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Game;

[TestFixture]
public class GameValidatorTests
{
    private GameValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new GameValidator();
    }

    [TestCase(null, TestName = "Name is null")]
    [TestCase("", TestName = "Name is empty")]
    [TestCase(" ", TestName = "Name is whitespace")]
    [TestCase("a", TestName = "Name is too short")]
    public void WhenNameIsEmptyTooShortOrNull_ShouldHaveError(string? name)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Name = name;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Test]
    public void WhenNameIsTooLong_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Name = StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestCase("Cyber Warriors 2077", TestName = "Valid game name")]
    [TestCase("FIFA Champions 2025", TestName = "Name with numbers")]
    [TestCase("Dragon's Legacy", TestName = "Name with apostrophe")]
    [TestCase("Street Racer Ultimate", TestName = "Multi-word name")]
    public void WhenNameIsValid_ShouldNotHaveError(string name)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Name = name;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
    
    [TestCase(null, TestName = "Key is null")]
    [TestCase("", TestName = "Key is empty")]
    [TestCase(" ", TestName = "Key is whitespace")]
    [TestCase("k", TestName = "Key is too short")]
    public void WhenKeyIsEmptyTooShortOrNull_ShouldHaveError(string? key)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Key = key;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Key);
    }

    [Test]
    public void WhenKeyIsTooLong_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Key = StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Key);
    }

    [TestCase("4uiru78rh6x84", TestName = "Valid alphanumeric key")]
    [TestCase("cyber-warriors-2077", TestName = "Valid key with hyphens")]
    [TestCase("dragon-legacy-2024", TestName = "Valid descriptive key")]
    [TestCase("street-racer-ultimate", TestName = "Valid multi-word key")]
    public void WhenKeyIsValid_ShouldNotHaveError(string key)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Key = key;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Key);
    }
    
    [TestCase(0, TestName = "Price is zero")]
    [TestCase(-1, TestName = "Price is negative")]
    [TestCase(-0.01, TestName = "Price is slightly negative")]
    public void WhenPriceIsZeroOrNegative_ShouldHaveError(decimal price)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Price = price;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [TestCase(29.99, TestName = "Valid low price")]
    [TestCase(59.99, TestName = "Valid standard price")]
    [TestCase(69.99, TestName = "Valid premium price")]
    [TestCase(0.01, TestName = "Valid minimum price")]
    [TestCase(999.99, TestName = "Valid high price")]
    public void WhenPriceIsValid_ShouldNotHaveError(decimal price)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Price = price;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Test]
    public void WhenDescriptionIsTooLong_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Description = StringConstants.LongerThatLongMaximum;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [TestCase(null, TestName = "Description is null")]
    [TestCase("", TestName = "Description is empty")]
    [TestCase("A futuristic action RPG set in a dystopian cyberpunk world.", TestName = "Valid description")]
    public void WhenDescriptionIsValidOrEmpty_ShouldNotHaveError(string? description)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Description = description;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [TestCase(-1, TestName = "Rating is negative")]
    [TestCase(-0.5, TestName = "Rating is slightly negative")]
    public void WhenRatingIsBelowMinimum_ShouldHaveCustomError(double rating)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Rating = rating;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Rating)
            .WithErrorMessage(ErrorMessages.IncorrectGameRating);
    }

    [TestCase(6, TestName = "Rating exceeds maximum")]
    [TestCase(10, TestName = "Rating way above maximum")]
    public void WhenRatingExceedsMaximum_ShouldHaveCustomError(double rating)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Rating = rating;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Rating)
            .WithErrorMessage(ErrorMessages.IncorrectGameRating);
    }

    [TestCase(0, TestName = "Minimum valid rating")]
    [TestCase(2.5, TestName = "Mid-range rating")]
    [TestCase(4.5, TestName = "High rating")]
    [TestCase(5, TestName = "Maximum valid rating")]
    public void WhenRatingIsValid_ShouldNotHaveError(double rating)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Rating = rating;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Rating);
    }
    
    [Test]
    public void WhenAgeRatingIsInvalid_ShouldHaveCustomError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.AgeRating = 99;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AgeRating)
            .WithErrorMessage(ErrorMessages.InvalidAgeRating);
    }

    [TestCase(3, TestName = "3+ rating valid rating")]
    [TestCase(7, TestName = "7+ rating valid rating")]
    [TestCase(12, TestName = "12+ rating valid rating")]
    [TestCase(16, TestName = "16+ rating valid rating")]
    [TestCase(18, TestName = "18+ rating valid rating")]

    public void WhenAgeRatingIsValid_ShouldNotHaveError(int ageRating)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.AgeRating = ageRating;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AgeRating);
    }

    [Test]
    public void WhenReleaseDateIsInFuture_ShouldHaveCustomError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.ReleaseDate = DateTime.Today.AddDays(1);
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDate)
            .WithErrorMessage(ErrorMessages.InvalidReleaseDate);
    }

    [Test]
    public void WhenReleaseDateIsWayInFuture_ShouldHaveCustomError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.ReleaseDate = DateTime.Today.AddYears(1);
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDate)
            .WithErrorMessage(ErrorMessages.InvalidReleaseDate);
    }

   [Test]
    public void WhenReleaseDateIsValid_ShouldNotHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.ReleaseDate = DateTime.Parse("2023-11-15");
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ReleaseDate);
    }

    [Test]
    public void WhenReleaseDateIsToday_ShouldNotHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.ReleaseDate = DateTime.Today;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ReleaseDate);
    }

    [Test]
    public void WhenPublisherIdIsEmpty_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.PublisherId = Guid.Empty;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
    }

    [Test]
    public void WhenPublisherIdIsValid_ShouldNotHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.PublisherId = Guid.NewGuid();
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherId);
    }

    [Test]
    public void WhenPlatformIdsIsEmpty_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.PlatformIds = [];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlatformIds);
    }

    [Test]
    public void WhenPlatformIdsIsNull_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.PlatformIds = null!;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlatformIds);
    }

    [Test]
    public void WhenPlatformIdsContainsEmptyGuid_ShouldHaveCustomError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.PlatformIds = [Guid.Empty, Guid.NewGuid()];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlatformIds)
            .WithErrorMessage("Platform Ids does not contain corrects ids");
    }

    [Test]
    public void WhenPlatformIdsAreValid_ShouldNotHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.PlatformIds = [Guid.NewGuid(), Guid.NewGuid()];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PlatformIds);
    }
    
    [Test]
    public void WhenGenreIdsIsEmpty_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.GenreIds = [];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GenreIds);
    }

    [Test]
    public void WhenGenreIdsIsNull_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.GenreIds = null!;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GenreIds);
    }
    
    [Test]
    public void WhenGenreIdsContainsEmptyGuid_ShouldHaveCustomError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.GenreIds = [Guid.NewGuid(), Guid.Empty];
        
        // Act
        var result = _validator.TestValidate(game);
        
        result.ShouldHaveValidationErrorFor(x => x.GenreIds)
            .WithErrorMessage("Genre Ids does not contain corrects ids");
    }

    [TestCase(1, TestName = "Single genre")]
    [TestCase(2, TestName = "Two genres")]
    [TestCase(5, TestName = "Multiple genres")]
    public void WhenGenreIdsAreValid_ShouldNotHaveError(int genreCount)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.GenreIds = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.GenreIds);
    }
    
    [Test]
    public void WhenPictureIsNull_ShouldNotHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Picture = null;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Picture);
    }
    
    [Test]
    public void WhenPictureIsTooSmall_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Picture = new byte[ValidationConstants.GamePicture.MinimumPictureSize - 1];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Picture)
            .WithErrorMessage(ErrorMessages.IncorrectPictureFormat);
    }

    [Test]
    public void WhenPictureIsTooLarge_ShouldHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Picture = new byte[ValidationConstants.GamePicture.MaximumPictureSize + 1];
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Picture)
            .WithErrorMessage(ErrorMessages.IncorrectPictureFormat);
    }

    [TestCase(new byte[] { 0xFF, 0xD8, 0xFF }, TestName = "JPEG format")]
    [TestCase(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, TestName = "PNG format")]
    [TestCase(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, TestName = "GIF87a format")]
    [TestCase(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, TestName = "VGIF89a format")]
    public void WhenPictureHasValidFormat_ShouldNotHaveError(byte[] pictureBytes)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Picture = TestData.Game.CreateGamePicture(pictureBytes);
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Picture);
    }

    [TestCase(new byte[] { 0x00, 0x00, 0x01, 0x00 }, TestName = "ICO format")]
    [TestCase(new byte[] { 0x49, 0x49, 0x2A, 0x00 }, TestName = "TIFF format")]
    [TestCase(new byte[] { 0x50, 0x4B, 0x03, 0x04 }, TestName = "ZIP format")]
    [TestCase(new byte[] { 0x25, 0x50, 0x44, 0x46 }, TestName = "PDF format")]
    public void WhenPictureHasInvalidFormat_ShouldHaveError(byte[] pictureBytes)
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Picture = TestData.Game.CreateGamePicture(pictureBytes);
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Picture)
            .WithErrorMessage(ErrorMessages.IncorrectPictureFormat);
    }
    
    [Test]
    public void WhenAllPropertiesAreValid_ShouldNotHaveError()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenMultiplePropertiesAreInvalid_ShouldHaveMultipleErrors()
    {
        var game = new GameRequestModel
        {
            Name = "",
            Key = "",
            Price = 0,
            Description = StringConstants.LongerThatLongMaximum,
            Rating = -1,
            AgeRating = 999, 
            ReleaseDate = DateTime.Today.AddDays(1),
            PublisherId = Guid.Empty, 
            PlatformIds = [], 
            GenreIds = [Guid.Empty]
        };
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Key);
        result.ShouldHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.Rating);
        result.ShouldHaveValidationErrorFor(x => x.AgeRating);
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDate);
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        result.ShouldHaveValidationErrorFor(x => x.PlatformIds);
        result.ShouldHaveValidationErrorFor(x => x.GenreIds);
    }
}