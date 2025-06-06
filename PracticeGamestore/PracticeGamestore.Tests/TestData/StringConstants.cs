using PracticeGamestore.Business.Constants;

namespace PracticeGamestore.Tests.TestData;

public static class StringConstants
{
    public static readonly string LongerThatShortMaximum = new('a', ValidationConstants.StringLength.ShortMaximum + 1);
    public static readonly string LongerThatLongMaximum = new('a', ValidationConstants.StringLength.LongMaximum + 1);
}