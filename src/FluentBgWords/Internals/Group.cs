namespace FluentBgWords.Internals;

internal sealed record Group(
    IList<string> Words,
    int NumeralWordCount);
