namespace PracticeGamestore.DataAccess.Constants;

public static class ValidationConstants
{
    public static class StringLength
    {
        public const int Minimum = 2;
        public const int ShortMaximum = 100;
        public const int LongMaximum = 2000;
    }
    public const int PageSize = 20;
    public const int MaximumPictureSize = 1048576;
    public const int MaxPhoneLength = 15;
    public const int MaxHashLength = 50;
    public const int MaxPasswordSaltLength = 36;
}