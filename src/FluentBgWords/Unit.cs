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
    internal string FormFor(long count)
        => count == 1 ? Singular : CountForm;
}
