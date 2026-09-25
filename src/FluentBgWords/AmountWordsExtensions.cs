namespace FluentBgWords;

/// <summary>Entry points of the fluent API.</summary>
public static class AmountWordsExtensions
{
    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount; at most 2 decimal places.</param>
    /// <remarks>
    /// This method does not validate the amount. It is validated when the text is produced by
    /// <see cref="AmountInWords.ToString"/>: the amount must be within ±999 999 999 999.99 and
    /// have at most 2 decimal places.
    /// </remarks>
    public static AmountInWords InWords(
        this decimal amount)
        => new(amount);

    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount.</param>
    /// <remarks>
    /// The supported range is ±999 999 999 999.99, which every <see cref="int"/> value is within.
    /// The amount is validated when the text is produced by <see cref="AmountInWords.ToString"/>.
    /// </remarks>
    public static AmountInWords InWords(
        this int amount)
        => new(amount);

    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount.</param>
    /// <remarks>
    /// This method does not validate the amount. It is validated when the text is produced by
    /// <see cref="AmountInWords.ToString"/>: the amount must be within ±999 999 999 999.
    /// </remarks>
    public static AmountInWords InWords(
        this long amount)
        => new(amount);
}
