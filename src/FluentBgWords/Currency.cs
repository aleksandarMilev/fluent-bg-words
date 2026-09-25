namespace FluentBgWords;

/// <summary>
/// A currency: a major unit (лев, евро), a minor unit (стотинка, цент) and their abbreviations.
/// </summary>
/// <param name="Major">The main unit, e.g. "лев" or "евро".</param>
/// <param name="Minor">The subunit (1/100 of the major unit), e.g. "стотинка" or "цент".</param>
/// <param name="MajorAbbreviation">Short form of the major unit, e.g. "лв." or "€".</param>
/// <param name="MinorAbbreviation">Short form of the minor unit, e.g. "ст." or "ц.".</param>
public sealed record Currency(
    Unit Major,
    Unit Minor,
    string MajorAbbreviation,
    string MinorAbbreviation)
{
    /// <summary>Bulgarian lev (BGN), for documents and data from before the euro changeover.</summary>
    public static readonly Currency Bgn = new(
        new Unit("лев", "лева", Gender.Masculine),
        new Unit("стотинка", "стотинки", Gender.Feminine),
        "лв.",
        "ст.");

    /// <summary>
    /// Euro (EUR) with "цент" as the subunit, as defined in art. 4 of the Euro Introduction Act.
    /// Abbreviations follow the official guidance: "е." and "ц.".
    /// </summary>
    public static readonly Currency Eur = new(
        new Unit("евро", "евро", Gender.Neuter),
        new Unit("цент", "цента", Gender.Masculine),
        "е.",
        "ц.");

    /// <summary>
    /// Euro (EUR) with "евроцент" as the subunit, the form common on invoices.
    /// Abbreviations follow the official guidance: "е." and "е.ц.".
    /// </summary>
    public static readonly Currency EurWithEurocents = new(
        new Unit("евро", "евро", Gender.Neuter),
        new Unit("евроцент", "евроцента", Gender.Masculine),
        "е.",
        "е.ц.");
}
