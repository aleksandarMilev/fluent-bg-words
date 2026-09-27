namespace FluentBgWords.Internals;

using FluentBgWords;

// The immutable settings behind both AmountInWords and AmountWordsFormatter. Write() is the only
// place that turns them into text, so the builder and the formatter can never disagree.
internal readonly record struct AmountWordsSettings(
    CurrencyDefinition? Currency,
    AmountFormat Format)
{
    public string Write(decimal amount)
        => AmountToWords.Convert(
            amount,
            this.Currency ?? CurrencyDefinition.Eur,
            this.Format);
}
