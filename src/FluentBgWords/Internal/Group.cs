namespace FluentBgWords.Internal;

internal sealed record Group(
    IList<string> Words,
    int NumeralWordCount);
