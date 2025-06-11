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
        $"Age rating must be between {ValidationConstants.GameRating.Min} and {ValidationConstants.GameRating.Max} inclusive.";

    public static string InvalidAgeRating =>
        $"Age rating must be one of: {string.Join(", ", ValidationConstants.AgeRatingValues)}";

    public static string IncorrectOrderByFields =>
        $"OrderBy fields must be one of: {string.Join(", ", ValidationConstants.OrderByFields)}";

    public static readonly string IncorrectPictureFormat =
        $"Picture must be a valid image format ({string.Join(", ", ValidationConstants.GamePicture.AllowedPictureFormats.Keys)}) " +
        $"and between {ValidationConstants.GamePicture.MinimumPictureSize} and {ValidationConstants.GamePicture.MaximumPictureSize} bytes.";

    public static readonly string InvalidGameFile = 
        $"Game file is required and must be between {ValidationConstants.GameFile.MinSize / 1024}KB and {ValidationConstants.GameFile.MaxSize / (1024 * 1024)}MB with one of the following extensions: {string.Join(", ", ValidationConstants.GameFile.AllowedExtensions)}.";
    
    public const string UnauthorizedAccess = "You don't have permission to perform this action.";
    public const string UnauthenticatedAccess = "You must be logged in to perform this action.";
    public const string IncorrectPageUrl = "Specified page URL is invalid.";
    public const string IncorrectOrdering = "Order by field must be either asc or desc.";
    public const string IncorrectQueryParameters = "Invalid query parameters";
    public const string InvalidReleaseDate = "Release date cannot be in future";
    public const string IncorrectName = "Name must contain only letters, hyphens, spaces, and apostrophes";
    public const string FailedFileUpload = "Failed to upload the file.";
}
