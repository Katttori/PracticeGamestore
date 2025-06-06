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

    public static readonly HashSet<string> OrderByFields = ["name", "price", "rating", "age", "release-date"];

    public const int PageSize = 20;

    public const int MaxPageSize = 100;
}