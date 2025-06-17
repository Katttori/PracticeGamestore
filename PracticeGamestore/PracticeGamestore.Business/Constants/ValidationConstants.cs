using System.Text.RegularExpressions;

namespace PracticeGamestore.Business.Constants;

public static class ValidationConstants
{
    public static class StringLength
    {
        public const int Minimum = DataAccess.Constants.ValidationConstants.StringLength.Minimum;
        public const int ShortMaximum = DataAccess.Constants.ValidationConstants.StringLength.ShortMaximum;
        public const int LongMaximum = DataAccess.Constants.ValidationConstants.StringLength.LongMaximum;
    }

    public static class GameRating
    {
        public const double Min = 0;
        public const double Max = 5;
    }

    public static readonly HashSet<int> AgeRatingValues = [3, 7, 12, 16, 18];
    
    public static readonly HashSet<string> UserRoleValues = ["Admin", "User", "Manager"];

    public static readonly HashSet<string> OrderByFields = ["name", "price", "rating", "age", "release-date"];

    public const int MaxPageSize = 100;
    
    public static class GamePicture
    {
        public static readonly Dictionary<string, byte[][]> AllowedPictureFormats = new()
        {
            { "JPEG", [[0xFF, 0xD8, 0xFF]] },
            { "PNG", [[0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]] },
            { "GIF", [
                [0x47, 0x49, 0x46, 0x38, 0x37, 0x61],
                [0x47, 0x49, 0x46, 0x38, 0x39, 0x61]
            ]}
        };

        public const int MinimumPictureSize = 102400;
        public const int MaximumPictureSize = DataAccess.Constants.ValidationConstants.MaximumPictureSize;
    }
    
    public static class GameFile
    {
        public static readonly List<string> AllowedExtensions = [".zip", ".rar", ".exe", ".msi", ".dmg", ".pkg", ".deb", ".rpm", ".7z"];
        public const long MaxSize = 200L * 1024 * 1024 * 1024;
        public const long MinSize = 1L * 1024 * 1024 * 1024;
    }
    
    public static class Password
    {
        public const int MinLength = 8;
        public const int MaxLength = 50;
        public static readonly Regex SpecialCharRegex = new(@"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]", RegexOptions.Compiled);
    }
    
    public static class PhoneNumber
    {
        public const int MaxLength = DataAccess.Constants.ValidationConstants.MaxPhoneLength;
        public const int MinLength = 10;
    }
}