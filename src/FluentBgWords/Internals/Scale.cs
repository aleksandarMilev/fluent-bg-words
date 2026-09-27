namespace FluentBgWords.Internals;

using FluentBgWords;

internal sealed record Scale(
    long Divisor,
    GrammaticalGender Gender,
    string Singular,
    string CountForm,
    bool OmitOne);
