namespace PracticeGamestore.Business.Constants;

public static class ErrorMessages
{
    
    public static string FailedToCreate(string entityName) => $"Failed to create {entityName}.";
    public static string NotFound(string entityName, Guid id) => $"{entityName} with id {id} does not exist.";
    public static string FailedToUpdate(string entityName, Guid id) => $"Failed to update {entityName} with id {id}.";

    public static string StringIsTooLong => "{PropertyName} cannot contain more than {MaxLength} characters.";
    public static string IncorrectLength => "{PropertyName} must be at least {MinLength} and at max {MaxLength} characters long.";
    public static string EmptyField => "{PropertyName} field cannot be empty.";
    public static string InvalidEnum => "Such {PropertyName} does not exist.";
    public static string MustBeGreaterThanZero => "{PropertyName} must be greater than zero.";
    public static string HasIncorrectIds => "{PropertyName} does not contain corrects ids";

    public static string FirstCannotBeGreaterThanSecond(string first, string second)
        => $"{first} cannot be greater than {second}";
    
    public static string InvalidPageSize 
        => $"Page size must be between 1 and {ValidationConstants.MaxPageSize}";
    
    public static string IncorrectGameRating =>
        $"Age rating must be between {ValidationConstants.GameRating.Min} and {ValidationConstants.GameRating.Max} inclusive.";

    public static string InvalidAgeRating =>
        $"Age rating must be one of: {string.Join(", ", ValidationConstants.AgeRatingValues)}";

    public static string IncorrectOrderByFields =>
        $"OrderBy fields must be one of: {string.Join(", ", ValidationConstants.OrderByFields)}";

    public const string UnauthorizedAccess = "You don't have permission to perform this action.";
    public const string UnauthenticatedAccess = "You must be logged in to perform this action.";
    public const string IncorrectPageUrl = "Specified page URL is invalid.";
    public const string IncorrectEmail = "Please provide a valid email.";
    public const string EmptyGuid = "The provided guid must not be empty.";
    public const string IncorrectOrdering = "Order by field must be either asc or desc.";
    public const string IncorrectQueryParameters = "Invalid query parameters";

}
