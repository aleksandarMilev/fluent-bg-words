namespace FluentBgWords.Internals;

using FluentBgWords;

internal sealed record Scale(
    long Divisor,
    GrammaticalGender Gender,
    string Singular,
    string Plural,
    bool OmitOne);
