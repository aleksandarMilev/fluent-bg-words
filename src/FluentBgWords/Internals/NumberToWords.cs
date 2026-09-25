namespace FluentBgWords.Internals;

using System.Diagnostics;
using FluentBgWords;

internal static class NumberToWords
{
    public const long MaxValue = 999_999_999_999;

    private const string And = "и";

    private static readonly string[] Units =
        ["", "", "", "три", "четири", "пет", "шест", "седем", "осем", "девет"];

    private static readonly string[] Teens =
        ["десет", "единадесет", "дванадесет", "тринадесет", "четиринадесет",
         "петнадесет", "шестнадесет", "седемнадесет", "осемнадесет", "деветнадесет"];

    private static readonly string[] Tens =
        ["", "", "двадесет", "тридесет", "четиридесет", "петдесет",
         "шестдесет", "седемдесет", "осемдесет", "деветдесет"];

    private static readonly string[] Hundreds =
        ["", "сто", "двеста", "триста", "четиристотин", "петстотин",
         "шестстотин", "седемстотин", "осемстотин", "деветстотин"];

    private static readonly Scale[] Scales =
    [
        // billions
        new(
            1_000_000_000,
            Gender.Masculine,
            "милиард",
            "милиарда",
            OmitOne: false),
        //millions
        new(
            1_000_000,
            Gender.Masculine,
            "милион",
            "милиона",
            OmitOne: false),
        // thousands
        new(
            1_000,
            Gender.Feminine,
            "хиляда",
            "хиляди",
            OmitOne: true),
    ];

    public static string Convert(
        long number,
        Gender gender = Gender.Masculine)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(number, MaxValue);
        ArgumentOutOfRangeException.ThrowIfLessThan(number, -MaxValue);

        if (number == 0)
        {
            return "нула";
        }

        if (number < 0)
        {
            return "минус " + Convert(-number, gender);
        }

        var groups = new List<Group>(4);

        foreach (var scale in Scales)
        {
            var value = (int)(number / scale.Divisor % 1_000);
            if (value > 0)
            {
                var scaleGroup = BuildScaleGroup(value, scale);
                groups.Add(scaleGroup);
            }
        }

        var units = (int)(number % 1_000);
        if (units > 0)
        {
            var group = BuildGroup(units, gender);
            groups.Add(group);
        }

        return Join(groups);
    }

    private static Group BuildScaleGroup(
        int value,
        Scale scale)
    {
        if (value == 1 && scale.OmitOne)
        {
            return new([scale.Singular], NumeralWordCount: 0);
        }

        var group = BuildGroup(value, scale.Gender);
        group
            .Words
            .Add(value == 1 ? scale.Singular : scale.Plural);

        return group;
    }

    private static Group BuildGroup(
        int value,
        Gender gender)
    {
        var words = Numerals(value, gender);
        var numeralWordCount = words.Count;

        if (words.Count >= 2)
        {
            words.Insert(words.Count - 1, And);
        }

        return new(words, numeralWordCount);
    }

    private static List<string> Numerals(
        int value,
        Gender gender)
    {
        var words = new List<string>(3);
        var hundreds = value / 100;
        var rest = value % 100;

        if (hundreds > 0)
        {
            words.Add(Hundreds[hundreds]);
        }

        if (rest is >= 10 and < 20)
        {
            words.Add(Teens[rest - 10]);
        }
        else
        {
            if (rest >= 20)
            {
                words.Add(Tens[rest / 10]);
            }

            if (rest % 10 > 0)
            {
                words.Add(Unit(rest % 10, gender));
            }
        }

        return words;
    }

    private static string Unit(int digit, Gender gender)
        => (digit, gender) switch
        {
            (1, Gender.Masculine) => "един",
            (1, Gender.Feminine) => "една",
            (1, Gender.Neuter) => "едно",
            (2, Gender.Masculine) => "два",
            (2, Gender.Feminine or Gender.Neuter) => "две",
            (_, Gender.Masculine or Gender.Feminine or Gender.Neuter) => Units[digit],
            _ => throw new UnreachableException($"Undefined gender: {gender}."),
        };

    private static string Join(List<Group> groups)
    {
        var words = new List<string>();

        for (var i = 0; i < groups.Count; i++)
        {
            var isLast = i == groups.Count - 1;
            var shouldBeAdded =
                isLast &&
                groups.Count > 1
                && groups[i].NumeralWordCount <= 1;

            if (shouldBeAdded)
            {
                words.Add(And);
            }

            words.AddRange(groups[i].Words);
        }

        return string.Join(' ', words);
    }
}
