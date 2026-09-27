namespace FluentBgWords;

using FluentBgWords.Internals;

/// <summary>
/// A currency: a major unit (лев, евро), a minor unit (стотинка, цент) and their abbreviations.
/// </summary>
/// <param name="Major">The main unit, e.g. "лев" or "евро".</param>
/// <param name="Minor">The subunit (1/100 of the major unit), e.g. "стотинка" or "цент".</param>
/// <param name="MajorAbbreviation">Short form of the major unit, e.g. "лв." or "€".</param>
/// <param name="MinorAbbreviation">Short form of the minor unit, e.g. "ст." or "ц.".</param>
public sealed record CurrencyDefinition(
    CurrencyUnit Major,
    CurrencyUnit Minor,
    string MajorAbbreviation,
    string MinorAbbreviation)
{
    /// <summary>Bulgarian lev (BGN), for documents and data from before the euro changeover.</summary>
    public static readonly CurrencyDefinition Bgn = new(
        new CurrencyUnit("лев", "лева", GrammaticalGender.Masculine),
        new CurrencyUnit("стотинка", "стотинки", GrammaticalGender.Feminine),
        "лв.",
        "ст.");

    /// <summary>
    /// Euro (EUR) with "цент" as the subunit, as defined in art. 4 of the Euro Introduction Act.
    /// Abbreviations follow the official guidance: "е." and "ц.".
    /// </summary>
    public static readonly CurrencyDefinition Eur = new(
        new CurrencyUnit("евро", "евро", GrammaticalGender.Neuter),
        new CurrencyUnit("цент", "цента", GrammaticalGender.Masculine),
        "е.",
        "ц.");

    /// <summary>
    /// Euro (EUR) with "евроцент" as the subunit, the form common on invoices.
    /// Abbreviations follow the official guidance: "е." and "е.ц.".
    /// </summary>
    public static readonly CurrencyDefinition EurWithEurocents = new(
        new CurrencyUnit("евро", "евро", GrammaticalGender.Neuter),
        new CurrencyUnit("евроцент", "евроцента", GrammaticalGender.Masculine),
        "е.",
        "е.ц.");

    private readonly CurrencyUnit major
        = Guard.AgainstNull(Major, nameof(Major));

    private readonly CurrencyUnit minor
        = Guard.AgainstNull(Minor, nameof(Minor));

    private readonly string majorAbbreviation
        = Guard.AgainstLeadingOrTrailingWhitespace(
            MajorAbbreviation,
            nameof(MajorAbbreviation));

    private readonly string minorAbbreviation
        = Guard.AgainstLeadingOrTrailingWhitespace(
            MinorAbbreviation,
            nameof(MinorAbbreviation));

    /// <summary>The main unit, e.g. "лев" or "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    public CurrencyUnit Major
    {
        get => this.major;
        init => this.major = Guard.AgainstNull(
            value,
            nameof(this.Major));
    }

    /// <summary>The subunit (1/100 of the major unit), e.g. "стотинка" or "цент".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    public CurrencyUnit Minor
    {
        get => this.minor;
        init => this.minor = Guard.AgainstNull(
            value,
            nameof(this.Minor));
    }

    /// <summary>Short form of the major unit, e.g. "лв." or "€".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string MajorAbbreviation
    {
        get => this.majorAbbreviation;
        init => this.majorAbbreviation = Guard.AgainstLeadingOrTrailingWhitespace(
            value,
            nameof(this.MajorAbbreviation));
    }

    /// <summary>Short form of the minor unit, e.g. "ст." or "ц.".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string MinorAbbreviation
    {
        get => this.minorAbbreviation;
        init => this.minorAbbreviation = Guard.AgainstLeadingOrTrailingWhitespace(
            value,
            nameof(this.MinorAbbreviation));
    }
}
