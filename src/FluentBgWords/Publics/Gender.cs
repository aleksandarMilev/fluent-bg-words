namespace FluentBgWords.Publics;

/// <summary>
/// Grammatical gender of the noun that follows a number.
/// Determines forms such as "един/една/едно" and "два/две".
/// </summary>
public enum Gender
{
    /// <summary>Masculine: "един лев", "два лева".</summary>
    Masculine,

    /// <summary>Feminine: "една стотинка", "две стотинки".</summary>
    Feminine,

    /// <summary>Neuter: "едно евро", "две евро".</summary>
    Neuter,
}
