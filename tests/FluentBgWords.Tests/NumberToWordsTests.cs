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
    [InlineData(199L, "сто деветдесет и девет")]
    [InlineData(200L, "двеста")]
    [InlineData(201L, "двеста и един")]
    [InlineData(300L, "триста")]
    [InlineData(400L, "четиристотин")]
    [InlineData(999L, "деветстотин деветдесет и девет")]
    // thousands
    [InlineData(1_000L, "хиляда")]
    [InlineData(1_001L, "хиляда и един")]
    [InlineData(1_100L, "хиляда и сто")]
    [InlineData(1_101L, "хиляда сто и един")]
    [InlineData(1_120L, "хиляда сто и двадесет")]
    [InlineData(1_999L, "хиляда деветстотин деветдесет и девет")]
    [InlineData(2_000L, "две хиляди")]
    [InlineData(2_001L, "две хиляди и един")]
    [InlineData(2_021L, "две хиляди двадесет и един")]
    [InlineData(2_500L, "две хиляди и петстотин")]
    [InlineData(10_001L, "десет хиляди и един")]
    [InlineData(12_000L, "дванадесет хиляди")]
    [InlineData(21_000L, "двадесет и една хиляди")]
    [InlineData(100_000L, "сто хиляди")]
    [InlineData(100_001L, "сто хиляди и един")]
    [InlineData(101_000L, "сто и една хиляди")]
    [InlineData(999_999L, "деветстотин деветдесет и девет хиляди деветстотин деветдесет и девет")]
    // millions and billions
    [InlineData(1_000_000L, "един милион")]
    [InlineData(1_000_001L, "един милион и един")]
    [InlineData(1_000_100L, "един милион и сто")]
    [InlineData(1_000_120L, "един милион сто и двадесет")]
    [InlineData(1_001_000L, "един милион и хиляда")]
    [InlineData(1_021_000L, "един милион двадесет и една хиляди")]
    [InlineData(1_200_000L, "един милион и двеста хиляди")]
    [InlineData(1_500_000L, "един милион и петстотин хиляди")]
    [InlineData(2_000_000L, "два милиона")]
    [InlineData(2_000_021L, "два милиона двадесет и един")]
    [InlineData(21_000_000L, "двадесет и един милиона")]
    [InlineData(1_000_000_000L, "един милиард")]
    [InlineData(1_000_000_001L, "един милиард и един")]
    [InlineData(1_000_001_000L, "един милиард и хиляда")]
    [InlineData(1_000_021_000L, "един милиард двадесет и една хиляди")]
    [InlineData(2_000_000_000L, "два милиарда")]
    [InlineData(21_000_000_000L, "двадесет и един милиарда")]
    [InlineData(22_000_000_000L, "двадесет и два милиарда")]
    // 3+ groups: expected values pin current behavior (single "и" before the last group).
    // NEEDS NATIVE VERIFICATION, see CODE_REVIEW.md TEST-01.
    [InlineData(1_001_001L, "един милион хиляда и един")]
    [InlineData(1_100_100L, "един милион сто хиляди и сто")]
    [InlineData(1_101_101L, "един милион сто и една хиляди сто и един")]
    [InlineData(21_021_021L, "двадесет и един милиона двадесет и една хиляди двадесет и един")]
    [InlineData(22_022_022L, "двадесет и два милиона двадесет и две хиляди двадесет и два")]
    [InlineData(101_101_101L, "сто и един милиона сто и една хиляди сто и един")]
    [InlineData(1_001_001_001L, "един милиард един милион хиляда и един")]
    [InlineData(111_111_111_111L, "сто и единадесет милиарда сто и единадесет милиона сто и единадесет хиляди сто и единадесет")]
    [InlineData(999_999_999_999L, "деветстотин деветдесет и девет милиарда деветстотин деветдесет и девет милиона деветстотин деветдесет и девет хиляди деветстотин деветдесет и девет")]

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
    [InlineData(22_000L, Gender.Neuter, "двадесет и две хиляди")]
    [InlineData(21_000_000L, Gender.Feminine, "двадесет и един милиона")]
    [InlineData(22_000_000_000L, Gender.Feminine, "двадесет и два милиарда")]
    // 3+ groups: expected values pin current behavior (single "и" before the last group).
    // NEEDS NATIVE VERIFICATION, see CODE_REVIEW.md TEST-01.
    [InlineData(1_001_001_001L, Gender.Neuter, "един милиард един милион хиляда и едно")]
    [InlineData(2_002_002_002L, Gender.Neuter, "два милиарда два милиона две хиляди и две")]
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
