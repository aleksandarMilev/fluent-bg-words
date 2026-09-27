namespace FluentBgWords;

using FluentBgWords.Internals;

/// <summary>
/// A currency unit that follows the amount, e.g. "лев", "стотинка", "евро" or "цент".
/// The values are validated on creation and in <see langword="with"/> expressions: both forms
/// must be non-empty with no leading or trailing whitespace, and the gender must be a defined
/// <see cref="GrammaticalGender"/> member.
/// </summary>
/// <param name="Singular">Form used for exactly one: "лев", "стотинка", "евро".</param>
/// <param name="CountForm">Form used for any other amount, including 21, 101, etc.: "лева", "стотинки", "евро".</param>
/// <param name="Gender">Grammatical gender; decides "един/една/едно" and "два/две".</param>
public sealed record CurrencyUnit(
    string Singular,
    string CountForm,
    GrammaticalGender Gender)
{
    private readonly string singular
        = Guard.AgainstBlankOrPadded(
            Singular,
            nameof(Singular));

    private readonly string countForm
        = Guard.AgainstBlankOrPadded(
            CountForm,
            nameof(CountForm));

    private readonly GrammaticalGender gender
        = Guard.AgainstUndefinedEnum(
            Gender,
            nameof(Gender));

    /// <summary>Form used for exactly one: "лев", "стотинка", "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string Singular
    {
        get => this.singular;
        init => this.singular = Guard.AgainstBlankOrPadded(
            value,
            nameof(this.Singular));
    }

    /// <summary>Form used for any other amount, including 21, 101, etc.: "лева", "стотинки", "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string CountForm
    {
        get => this.countForm;
        init => this.countForm = Guard.AgainstBlankOrPadded(
            value,
            nameof(this.CountForm));
    }

    /// <summary>Grammatical gender; decides "един/една/едно" and "два/две".</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a defined <see cref="GrammaticalGender"/> member.</exception>
    public GrammaticalGender Gender
    {
        get => this.gender;
        init => this.gender = Guard.AgainstUndefinedEnum(
            value,
            nameof(this.Gender));
    }

    internal string FormFor(long count)
        => count == 1 ? this.Singular : this.CountForm;
}
