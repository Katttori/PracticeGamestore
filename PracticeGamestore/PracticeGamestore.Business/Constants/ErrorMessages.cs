namespace PracticeGamestore.Business.Constants;

public static class ErrorMessages
{
    public static string FailedToCreate(string entityName) => $"Failed to create {entityName}.";
    public static string NotFound(string entityName, Guid id) => $"{entityName} with id {id} does not exist.";
    public static string FailedToUpdate(string entityName, Guid id) => $"Failed to update {entityName} with id {id}.";

    public static string HasIncorrectIds => "{PropertyName} does not contain corrects ids";

    public static string FirstCannotBeGreaterThanSecond(string first, string second)
        => $"{first} cannot be greater than {second}";
    
    public static string IncorrectGameRating =>
        $"Game rating must be between {ValidationConstants.GameRating.Min} and {ValidationConstants.GameRating.Max} inclusive.";

    public static string InvalidAgeRating =>
        $"Age rating must be one of: {string.Join(", ", ValidationConstants.AgeRatingValues)}";
    
    public static string InvalidRole =>
        $"Role must be one of: {string.Join(", ", ValidationConstants.UserRoleValues)}";

    public static string IncorrectOrderByFields =>
        $"OrderBy fields must be one of: {string.Join(", ", ValidationConstants.OrderByFields)}";

    public static readonly string IncorrectPictureFormat =
        $"Picture must be a valid image format ({string.Join(", ", ValidationConstants.GamePicture.AllowedPictureFormats.Keys)}) " +
        $"and between {ValidationConstants.GamePicture.MinimumPictureSize} and {ValidationConstants.GamePicture.MaximumPictureSize} bytes.";

    public static readonly string InvalidGameFile = 
        $"Game file is required and must be between {ValidationConstants.GameFile.MinSize / 1024}KB and {ValidationConstants.GameFile.MaxSize / (1024 * 1024)}MB with one of the following extensions: {string.Join(", ", ValidationConstants.GameFile.AllowedExtensions)}.";
    
    public const string UnauthorizedAccess = "You don't have permission to perform this action.";
    public const string Unauthorized = "Unauthorized! Your credentilas are invalid.";
    public const string UnauthenticatedAccess = "You must be logged in to perform this action.";
    public const string IncorrectPageUrl = "Specified page URL is invalid.";
    public const string IncorrectOrdering = "Order by field must be either asc or desc.";
    public const string IncorrectQueryParameters = "Invalid query parameters";
    public const string InvalidReleaseDate = "Release date cannot be in future";
    public const string IncorrectName = "Name must contain only letters, hyphens, spaces, and apostrophes";
    public const string FailedFileUpload = "Failed to upload the file.";
    public const string GlobalError = "something happened?";
    public const string MissingEmailHeader = "Missing X-User-Email header.";
    public const string MissingLocationHeader = "Missing X-Location-Country header.";
    public const string BlacklistedUser = "you are banned :)";
    public const string IncorrectPhoneNumber = "Phone number must be in the format +1234567890 or 1234567890, and must not contain letters or special characters.";
    public const string RegistrationFailed = "Registartion failed.";
    public const string InvalidRegistrationCountry = "Invalid country was specified during registration.";
    public const string PasswordRequired = "Password is required.";
    public const string InsecurePassword =
        "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character";
}
