using FluentBgWords.Internals;

namespace FluentBgWords;

/// <summary>
/// A currency unit that follows the amount, e.g. "лев", "стотинка", "евро" or "цент".
/// </summary>
/// <param name="Singular">Form used for exactly one: "лев", "стотинка", "евро".</param>
/// <param name="CountForm">Form used for any other amount, including 21, 101, etc.: "лева", "стотинки", "евро".</param>
/// <param name="Gender">Grammatical gender; decides "един/една/едно" and "два/две".</param>
public sealed record Unit(
    string Singular,
    string CountForm,
    Gender Gender)
{
    private readonly string singular
        = Guard.AgainstLeadingOrTrailingWhitespace(
            Singular,
            nameof(Singular));

    private readonly string countForm
        = Guard.AgainstLeadingOrTrailingWhitespace(
            CountForm,
            nameof(CountForm));

    private readonly Gender gender
        = Guard.AgainstUndefinedEnum(
            Gender,
            nameof(Gender));

    /// <summary>Form used for exactly one: "лев", "стотинка", "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string Singular
    {
        get => this.singular;
        init => this.singular = Guard.AgainstLeadingOrTrailingWhitespace(
            value,
            nameof(Singular));
    }

    /// <summary>Form used for any other amount, including 21, 101, etc.: "лева", "стотинки", "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string CountForm
    {
        get => this.countForm;
        init => this.countForm = Guard.AgainstLeadingOrTrailingWhitespace(
            value,
            nameof(CountForm));
    }

    /// <summary>Grammatical gender; decides "един/една/едно" and "два/две".</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a defined <see cref="FluentBgWords.Gender"/> member.</exception>
    public Gender Gender
    {
        get => this.gender;
        init => this.gender = Guard.AgainstUndefinedEnum(
            value,
            nameof(Gender));
    }

    internal string FormFor(long count)
        => count == 1 ? Singular : CountForm;
}
