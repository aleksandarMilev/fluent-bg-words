using System.Globalization;

namespace FluentBgWords.Tests;

public class CultureInvarianceTests
{
    [Theory]
    [InlineData("bg-BG")]
    [InlineData("en-US")]
    [InlineData("tr-TR")]
    [InlineData("ar-SA")]
    public void InWords_AnyCurrentCulture_WritesSameText(string culture)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            Assert.Equal(
                "Минус сто шестдесет и седем лв. и 42 ст.",
                (-167.42m)
                    .InWords()
                    .AsBgn()
                    .WithSubunitsAsDigits()
                    .Abbreviated()
                    .Capitalized()
                    .ToString());

            Assert.Equal(
                "нула евро и 5 цента",
                0.05m.InWords().WithSubunitsAsDigits().ToString());

            Assert.Equal(
                "Един милион хиляда и едно евро и един евроцент",
                1_001_001.01m.InWords().AsEurWithEurocents().Capitalized().ToString());
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }
}
