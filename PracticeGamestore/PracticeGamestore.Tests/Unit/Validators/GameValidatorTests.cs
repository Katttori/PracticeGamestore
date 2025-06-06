using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Validators;
using PracticeGamestore.Models.Game;
using PracticeGamestore.Tests.TestData;

namespace PracticeGamestore.Tests.Unit.Validators;

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
    public void ShouldHaveError_WhenNameIsEmptyTooShortOrNull(string? name)
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
    public void ShouldHaveError_WhenNameIsTooLong()
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
    public void ShouldNotHaveError_WhenNameIsValid(string name)
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
    public void ShouldHaveError_WhenKeyIsEmptyTooShortOrNull(string? key)
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
    public void ShouldHaveError_WhenKeyIsTooLong()
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
    public void ShouldNotHaveError_WhenKeyIsValid(string key)
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
    public void ShouldHaveError_WhenPriceIsZeroOrNegative(decimal price)
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
    public void ShouldNotHaveError_WhenPriceIsValid(decimal price)
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
    public void ShouldHaveError_WhenDescriptionIsTooLong()
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
    public void ShouldNotHaveError_WhenDescriptionIsValidOrEmpty(string? description)
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
    public void ShouldHaveCustomError_WhenRatingIsBelowMinimum(double rating)
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
    public void ShouldHaveCustomError_WhenRatingExceedsMaximum(double rating)
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
    public void ShouldNotHaveError_WhenRatingIsValid(double rating)
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
    public void ShouldHaveCustomError_WhenAgeRatingIsInvalid()
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

    public void ShouldNotHaveError_WhenAgeRatingIsValid(int ageRating)
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
    public void ShouldHaveCustomError_WhenReleaseDateIsInFuture()
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
    public void ShouldHaveCustomError_WhenReleaseDateIsWayInFuture()
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
    public void ShouldNotHaveError_WhenReleaseDateIsValid()
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
    public void ShouldNotHaveError_WhenReleaseDateIsToday()
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
    public void ShouldHaveError_WhenPublisherIdIsEmpty()
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
    public void ShouldNotHaveError_WhenPublisherIdIsValid()
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
    public void ShouldHaveError_WhenPlatformIdsIsEmpty()
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
    public void ShouldHaveError_WhenPlatformIdsIsNull()
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
    public void ShouldHaveCustomError_WhenPlatformIdsContainsEmptyGuid()
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
    public void ShouldNotHaveError_WhenPlatformIdsAreValid()
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
    public void ShouldHaveError_WhenGenreIdsIsEmpty()
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
    public void ShouldHaveError_WhenGenreIdsIsNull()
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
    public void ShouldHaveCustomError_WhenGenreIdsContainsEmptyGuid()
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
    public void ShouldNotHaveError_WhenGenreIdsAreValid(int genreCount)
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
    public void ShouldNotHaveError_WhenAllPropertiesAreValid()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenMultiplePropertiesAreInvalid()
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
    
    [Test]
    public void ShouldNotHaveError_WhenPictureIsNull()
    {
        // Arrange
        var game = TestData.Game.GenerateGameRequestModel();
        game.Picture = null;
        
        // Act
        var result = _validator.TestValidate(game);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Picture);
    }
}