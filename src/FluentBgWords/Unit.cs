using FluentBgWords.Internals;

namespace FluentBgWords;

/// <summary>
/// A unit that follows a number, e.g. "лев", "стотинка" or "килограм".
/// </summary>
/// <param name="Singular">Form used for exactly one: "лев", "стотинка", "евро".</param>
/// <param name="CountForm">Form used for any other amount, including 21, 101, etc.: "лева", "стотинки", "евро".</param>
/// <param name="Gender">Grammatical gender; decides "един/една/едно" and "два/две".</param>
public sealed record Unit(
    string Singular,
    string CountForm,
    Gender Gender)
{
    private readonly string singular = Guard.Text(Singular, nameof(Singular));

    private readonly string countForm = Guard.Text(CountForm, nameof(CountForm));

    /// <summary>Form used for exactly one: "лев", "стотинка", "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string Singular
    {
        get => this.singular;
        init => this.singular = Guard.Text(value, nameof(Singular));
    }

    /// <summary>Form used for any other amount, including 21, 101, etc.: "лева", "стотинки", "евро".</summary>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    /// <exception cref="ArgumentException">The value is empty, whitespace, or has leading or trailing whitespace.</exception>
    public string CountForm
    {
        get => this.countForm;
        init => this.countForm = Guard.Text(value, nameof(CountForm));
    }

    internal string FormFor(long count)
        => count == 1 ? Singular : CountForm;
}
