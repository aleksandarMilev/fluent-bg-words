namespace FluentBgWords.Internals;

using Publics;

internal sealed record Scale(
    long Divisor,
    Gender Gender,
    string Singular,
    string Plural,
    bool OmitOne);
