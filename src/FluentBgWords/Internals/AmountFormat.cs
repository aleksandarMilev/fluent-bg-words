namespace FluentBgWords.Internals;

internal readonly record struct AmountFormat(
    bool SubunitsAsDigits = false,
    bool Abbreviated = false);
