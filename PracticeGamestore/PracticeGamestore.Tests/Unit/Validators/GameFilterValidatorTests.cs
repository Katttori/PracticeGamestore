using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Validators;
using PracticeGamestore.Business.DataTransferObjects.Filtering;

namespace PracticeGamestore.Tests.Unit.Validators;

[TestFixture]
public class GameFilterValidatorTests
{
    private GameFilterValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new GameFilterValidator();
    }

    [TestCase(-10, TestName = "MinPrice is negative")]
    [TestCase(-0.01, TestName = "MinPrice is slightly negative")]
    public void ShouldHaveError_WhenMinPriceIsNegative(decimal minPrice)
    {
        // Arrange
        var filter = new GameFilter { MinPrice = minPrice };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MinPrice);
    }

    [TestCase(-5, TestName = "MaxPrice is negative")]
    [TestCase(-1, TestName = "MaxPrice is minus one")]
    public void ShouldHaveError_WhenMaxPriceIsNegative(decimal maxPrice)
    {
        // Arrange
        var filter = new GameFilter { MaxPrice = maxPrice };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MaxPrice);
    }

    [Test]
    public void ShouldHaveCustomError_WhenMinPriceIsGreaterThanMaxPrice()
    {
        // Arrange
        var filter = new GameFilter { MinPrice = 100, MaxPrice = 50 };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Minimum price", "maximum price"));
    }

    [TestCase(0, 0, TestName = "Both prices are zero")]
    [TestCase(30, 60, TestName = "Valid price range")]
    [TestCase(10, 100, TestName = "Large price range")]
    [TestCase(50, 50, TestName = "Equal min and max price")]
    public void ShouldNotHaveError_WhenPriceRangeIsValid(decimal minPrice, decimal maxPrice)
    {
        // Arrange
        var filter = new GameFilter { MinPrice = minPrice, MaxPrice = maxPrice };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.MinPrice);
        result.ShouldNotHaveValidationErrorFor(x => x.MaxPrice);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [TestCase(null, 50, TestName = "Only MaxPrice provided")]
    [TestCase(10, null, TestName = "Only MinPrice provided")]
    [TestCase(null, null, TestName = "No prices provided")]
    public void ShouldNotHaveError_WhenOnlyOnePriceIsProvided(decimal? minPrice, decimal? maxPrice)
    {
        // Arrange
        var filter = new GameFilter { MinPrice = minPrice, MaxPrice = maxPrice };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
    
    [TestCase(-1, TestName = "RatingFrom is negative")]
    [TestCase(-0.5, TestName = "RatingFrom is slightly negative")]
    public void ShouldHaveError_WhenRatingFromIsNegative(double ratingFrom)
    {
        // Arrange
        var filter = new GameFilter { RatingFrom = ratingFrom };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RatingFrom);
    }

    [TestCase(6, TestName = "RatingFrom exceeds maximum")]
    [TestCase(7.5, TestName = "RatingFrom is way above maximum")]
    public void ShouldHaveError_WhenRatingFromExceedsMaximum(double ratingFrom)
    {
        // Arrange
        var filter = new GameFilter { RatingFrom = ratingFrom };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RatingFrom);
    }

    [TestCase(-0.5, TestName = "RatingTo is negative")]
    [TestCase(-1, TestName = "RatingTo is minus one")]
    public void ShouldHaveError_WhenRatingToIsNegative(double ratingTo)
    {
        // Arrange
        var filter = new GameFilter { RatingTo = ratingTo };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RatingTo);
    }

    [TestCase(7.5, TestName = "RatingTo exceeds maximum")]
    [TestCase(10, TestName = "RatingTo is way above maximum")]
    public void ShouldHaveError_WhenRatingToExceedsMaximum(double ratingTo)
    {
        // Arrange
        var filter = new GameFilter { RatingTo = ratingTo };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RatingTo);
    }

    [Test]
    public void ShouldHaveCustomError_WhenRatingFromIsGreaterThanRatingTo()
    {
        // Arrange
        var filter = new GameFilter { RatingFrom = 4.5, RatingTo = 3.0 };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Rating from", "rating to"));
    }

    [TestCase(0, 5, TestName = "Full rating range")]
    [TestCase(2.5, 4.0, TestName = "Mid-range ratings")]
    [TestCase(4.0, 4.0, TestName = "Equal rating from and to")]
    [TestCase(null, 4.0, TestName = "Only RatingTo provided")]
    [TestCase(2.0, null, TestName = "Only RatingFrom provided")]
    public void ShouldNotHaveError_WhenRatingRangeIsValid(double? ratingFrom, double? ratingTo)
    {
        // Arrange
        var filter = new GameFilter { RatingFrom = ratingFrom, RatingTo = ratingTo };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        if (ratingFrom.HasValue) result.ShouldNotHaveValidationErrorFor(x => x.RatingFrom);
        if (ratingTo.HasValue) result.ShouldNotHaveValidationErrorFor(x => x.RatingTo);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Test]
    public void ShouldHaveCustomError_WhenReleaseDateStartIsInFuture()
    {
        // Arrange
        var filter = new GameFilter { ReleaseDateStart = DateTime.Today.AddDays(1) };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDateStart)
            .WithErrorMessage(ErrorMessages.InvalidReleaseDate);
    }

    [Test]
    public void ShouldHaveCustomError_WhenReleaseDateEndIsInFuture()
    {
        // Arrange
        var filter = new GameFilter { ReleaseDateEnd = DateTime.Today.AddDays(10) };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDateEnd)
            .WithErrorMessage(ErrorMessages.InvalidReleaseDate);
    }

    [Test]
    public void ShouldHaveCustomError_WhenStartDateIsAfterEndDate()
    {
        // Arrange
        var filter = new GameFilter 
        { 
            ReleaseDateStart = DateTime.Today.AddDays(-1),
            ReleaseDateEnd = DateTime.Today.AddDays(-10)
        };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Start release date", "end release date"));
    }

    [TestCase("2024-01-01", "2024-12-31", TestName = "Valid date range for 2024")]
    [TestCase("2023-06-15", "2023-06-15", TestName = "Same start and end date")]
    public void ShouldNotHaveError_WhenDateRangeIsValid(string startDateStr, string endDateStr)
    {
        // Arrange
        var filter = new GameFilter 
        { 
            ReleaseDateStart = DateTime.Parse(startDateStr),
            ReleaseDateEnd = DateTime.Parse(endDateStr)
        };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ReleaseDateStart);
        result.ShouldNotHaveValidationErrorFor(x => x.ReleaseDateEnd);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Test]
    public void ShouldNotHaveError_WhenOnlyStartDateProvided()
    {
        // Arrange
        var filter = new GameFilter { ReleaseDateStart = DateTime.Today.AddDays(-30) };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ReleaseDateStart);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Test]
    public void ShouldNotHaveError_WhenOnlyEndDateProvided()
    {
        // Arrange
        var filter = new GameFilter { ReleaseDateEnd = DateTime.Today };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ReleaseDateEnd);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
    
    [TestCase("invalid", TestName = "Invalid order direction")]
    [TestCase("ascending", TestName = "Wrong order term")]
    [TestCase("descending", TestName = "Wrong order term")]
    [TestCase("up", TestName = "Random order value")]
    public void ShouldHaveCustomError_WhenOrderDirectionIsInvalid(string order)
    {
        // Arrange
        var filter = new GameFilter { Order = order };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Order)
            .WithErrorMessage(ErrorMessages.IncorrectOrdering);
    }

    [TestCase("asc", TestName = "Valid ascending order")]
    [TestCase("desc", TestName = "Valid descending order")]
    [TestCase("ASC", TestName = "Valid ascending order uppercase")]
    [TestCase("DESC", TestName = "Valid descending order uppercase")]
    [TestCase("Asc", TestName = "Valid ascending order mixed case")]
    public void ShouldNotHaveError_WhenOrderDirectionIsValid(string order)
    {
        // Arrange
        var filter = new GameFilter { Order = order };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Order);
    }

    [Test]
    public void ShouldHaveCustomError_WhenOrderByFieldsAreInvalid()
    {
        // Arrange
        var filter = new GameFilter { OrderBy = new List<string> { "invalid-field", "another-invalid" } };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.OrderBy)
            .WithErrorMessage(ErrorMessages.IncorrectOrderByFields);
    }

    [Test]
    public void ShouldNotHaveError_WhenOrderByFieldsAreValid()
    {
        // Arrange - Assuming these are valid fields based on ValidationConstants
        var filter = new GameFilter { OrderBy = new List<string> { "price", "rating", "name" } };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.OrderBy);
    }

    [Test]
    public void ShouldNotHaveError_WhenOrderByIsEmpty()
    {
        // Arrange
        var filter = new GameFilter { OrderBy = new List<string>() };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.OrderBy);
    }

    [Test]
    public void ShouldNotHaveError_WhenOrderByIsNull()
    {
        // Arrange
        var filter = new GameFilter { OrderBy = null };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.OrderBy);
    }
    
    [TestCase(new[] { 999 }, TestName = "Invalid age rating")]
    [TestCase(new[] { -1 }, TestName = "Negative age rating")]
    [TestCase(new[] { 12, 999 }, TestName = "Mix of valid and invalid ratings")]
    public void ShouldHaveCustomError_WhenAgeRatingsAreInvalid(int[] ageRatings)
    {
        // Arrange
        var filter = new GameFilter { Age = ageRatings.ToList() };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Age)
            .WithErrorMessage(ErrorMessages.InvalidAgeRating);
    }

    [TestCase(new[] { 12 }, TestName = "Single valid age rating")]
    [TestCase(new[] { 12, 16 }, TestName = "Multiple valid age ratings")]
    [TestCase(new[] { 3, 7, 12, 16, 18 }, TestName = "All valid age ratings")]
    public void ShouldNotHaveError_WhenAgeRatingsAreValid(int[] ageRatings)
    {
        // Arrange
        var filter = new GameFilter { Age = ageRatings.ToList() };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Age);
    }

    [Test]
    public void ShouldNotHaveError_WhenAgeIsEmpty()
    {
        // Arrange
        var filter = new GameFilter { Age = new List<int>() };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Age);
    }

    [Test]
    public void ShouldNotHaveError_WhenAgeIsNull()
    {
        // Arrange
        var filter = new GameFilter { Age = null };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Age);
    }

    [TestCase(-1, TestName = "Page is negative")]
    [TestCase(0, TestName = "Page is zero")]
    public void ShouldHaveError_WhenPageIsInvalid(int page)
    {
        // Arrange
        var filter = new GameFilter { Page = page };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [TestCase(-5, TestName = "PageSize is negative")]
    [TestCase(0, TestName = "PageSize is zero")]
    public void ShouldHaveError_WhenPageSizeIsInvalid(int pageSize)
    {
        // Arrange
        var filter = new GameFilter { PageSize = pageSize };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Test]
    public void ShouldHaveError_WhenPageSizeExceedsMaximum()
    {
        // Arrange
        var filter = new GameFilter { PageSize = ValidationConstants.MaxPageSize + 1 };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [TestCase(1, 1, TestName = "Minimum valid pagination")]
    [TestCase(1, 10, TestName = "Common pagination")]
    [TestCase(5, 20, TestName = "Larger pagination")]
    public void ShouldNotHaveError_WhenPaginationIsValid(int page, int pageSize)
    {
        // Arrange
        var filter = new GameFilter { Page = page, PageSize = pageSize };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Page);
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }

    [Test]
    public void ShouldNotHaveError_WhenPageSizeIsAtMaximum()
    {
        // Arrange
        var filter = new GameFilter { PageSize = ValidationConstants.MaxPageSize };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }

    [Test]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var filter = new GameFilter { Name = new string('a', ValidationConstants.StringLength.ShortMaximum + 1) };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestCase("cyber", TestName = "Short game name")]
    [TestCase("Call of Duty", TestName = "Normal game name")]
    [TestCase("", TestName = "Empty name")]
    [TestCase(null, TestName = "Null name")]
    public void ShouldNotHaveError_WhenNameIsValid(string? name)
    {
        // Arrange
        var filter = new GameFilter { Name = name };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldNotHaveError_WhenNameIsAtMaximumLength()
    {
        // Arrange
        var filter = new GameFilter { Name = new string('a', ValidationConstants.StringLength.ShortMaximum) };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
    

    [Test]
    public void ShouldNotHaveError_WhenAllPropertiesAreValid()
    {
        // Arrange
        var filter = new GameFilter 
        { 
            Name = "cyber",
            MinPrice = 20,
            MaxPrice = 70,
            RatingFrom = 4.0,
            RatingTo = 5.0,
            Age = [16, 18],
            ReleaseDateStart = new DateTime(2023, 1, 1),
            ReleaseDateEnd = new DateTime(2024, 12, 31),
            OrderBy = ["rating", "price"],
            Order = "desc",
            Page = 1,
            PageSize = 10
        };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenMultiplePropertiesAreInvalid()
    {
        // Arrange
        var filter = new GameFilter 
        { 
            MinPrice = -10,
            MaxPrice = -5,
            RatingFrom = 6,
            RatingTo = -1,
            ReleaseDateStart = DateTime.Today.AddDays(1),
            Order = "invalid",
            OrderBy = ["invalid-field"],
            Age = [999], 
            Page = 0,  
            PageSize = 0  
        };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MinPrice);
        result.ShouldHaveValidationErrorFor(x => x.MaxPrice);
        result.ShouldHaveValidationErrorFor(x => x.RatingFrom);
        result.ShouldHaveValidationErrorFor(x => x.RatingTo);
        result.ShouldHaveValidationErrorFor(x => x.ReleaseDateStart);
        result.ShouldHaveValidationErrorFor(x => x.Order);
        result.ShouldHaveValidationErrorFor(x => x.OrderBy);
        result.ShouldHaveValidationErrorFor(x => x.Age);
        result.ShouldHaveValidationErrorFor(x => x.Page);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Test]
    public void ShouldNotHaveError_WhenFilterIsEmpty()
    {
        // Arrange
        var filter = new GameFilter();
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Test]
    public void ShouldNotHaveError_WhenNoFieldsAreProvided()
    {
        // Arrange
        var filter = new GameFilter();
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldNotHaveError_WhenBoundaryValuesAreUsed()
    {
        // Arrange
        var filter = new GameFilter 
        { 
            MinPrice = 0,
            MaxPrice = 0,
            RatingFrom = ValidationConstants.GameRating.Min,
            RatingTo = ValidationConstants.GameRating.Max,
            ReleaseDateStart = DateTime.Today,
            ReleaseDateEnd = DateTime.Today,
            Page = 1,
            PageSize = 1
        };
        
        // Act
        var result = _validator.TestValidate(filter);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}