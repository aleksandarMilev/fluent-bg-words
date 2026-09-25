namespace FluentBgWords.Internals;

internal sealed record Group(
    IReadOnlyList<string> Words,
    int NumeralWordCount);
