using FluentBgWords.Internals;

namespace FluentBgWords.Tests;

public class NumberToWordsTests
{
    [Theory]
    // units, teens and tens
    [InlineData(0L, "нула")]
    [InlineData(1L, "един")]
    [InlineData(2L, "два")]
    [InlineData(9L, "девет")]
    [InlineData(10L, "десет")]
    [InlineData(11L, "единадесет")]
    [InlineData(12L, "дванадесет")]
    [InlineData(19L, "деветнадесет")]
    [InlineData(20L, "двадесет")]
    [InlineData(21L, "двадесет и един")]
    [InlineData(99L, "деветдесет и девет")]
    // hundreds
    [InlineData(100L, "сто")]
    [InlineData(101L, "сто и един")]
    [InlineData(110L, "сто и десет")]
    [InlineData(111L, "сто и единадесет")]
    [InlineData(120L, "сто и двадесет")]
    [InlineData(121L, "сто двадесет и един")]
    [InlineData(200L, "двеста")]
    [InlineData(300L, "триста")]
    [InlineData(400L, "четиристотин")]
    [InlineData(999L, "деветстотин деветдесет и девет")]
    // thousands
    [InlineData(1_000L, "хиляда")]
    [InlineData(1_001L, "хиляда и един")]
    [InlineData(1_100L, "хиляда и сто")]
    [InlineData(1_101L, "хиляда сто и един")]
    [InlineData(1_120L, "хиляда сто и двадесет")]
    [InlineData(2_000L, "две хиляди")]
    [InlineData(2_021L, "две хиляди двадесет и един")]
    [InlineData(2_500L, "две хиляди и петстотин")]
    [InlineData(12_000L, "дванадесет хиляди")]
    [InlineData(21_000L, "двадесет и една хиляди")]
    [InlineData(100_000L, "сто хиляди")]
    [InlineData(101_000L, "сто и една хиляди")]
    [InlineData(999_999L, "деветстотин деветдесет и девет хиляди деветстотин деветдесет и девет")]
    // millions and billions
    [InlineData(1_000_000L, "един милион")]
    [InlineData(1_000_001L, "един милион и един")]
    [InlineData(1_001_000L, "един милион и хиляда")]
    [InlineData(1_500_000L, "един милион и петстотин хиляди")]
    [InlineData(2_000_000L, "два милиона")]
    [InlineData(21_000_000L, "двадесет и един милиона")]
    [InlineData(1_000_000_000L, "един милиард")]
    [InlineData(2_000_000_000L, "два милиарда")]

    public void Convert_Masculine_ReturnsExpectedWords(
        long number,
        string expected)
        => Assert.Equal(
            expected,
            NumberToWords.Convert(number, Gender.Masculine));

    [Theory]
    [InlineData(1L, Gender.Feminine, "една")]
    [InlineData(2L, Gender.Feminine, "две")]
    [InlineData(21L, Gender.Feminine, "двадесет и една")]
    [InlineData(1_001L, Gender.Feminine, "хиляда и една")]
    [InlineData(1L, Gender.Neuter, "едно")]
    [InlineData(2L, Gender.Neuter, "две")]
    [InlineData(22L, Gender.Neuter, "двадесет и две")]
    [InlineData(2_000_001L, Gender.Neuter, "два милиона и едно")]
    public void Convert_GenderAffectsOnlyLastGroup(
        long number,
        Gender gender,
        string expected)
        => Assert.Equal(
            expected,
            NumberToWords.Convert(number, gender));

    [Fact]
    public void Convert_Negative_PrefixesMinus()
        => Assert.Equal(
            "минус двадесет и един",
            NumberToWords.Convert(-21));

    [Fact]
    public void Convert_AboveMaxValue_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => NumberToWords.Convert(NumberToWords.MaxValue + 1));
}
