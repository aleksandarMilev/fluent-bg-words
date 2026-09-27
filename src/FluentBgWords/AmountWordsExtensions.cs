namespace FluentBgWords;

/// <summary>Entry points of the fluent API.</summary>
public static class AmountWordsExtensions
{
    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount; at most 2 decimal places.</param>
    /// <remarks>
    /// The amount is validated here, so an invalid amount fails where it is passed in: it must be
    /// within ±999 999 999 999.99 and have at most 2 decimal places. The resulting builder's
    /// <see cref="AmountInWords.ToString"/> does not throw.
    /// </remarks>
    /// <exception cref="ArgumentException"><paramref name="amount"/> has more than 2 decimal places.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is outside ±999 999 999 999.99.</exception>
    public static AmountInWords InWords(
        this decimal amount)
        => new(amount);

    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount.</param>
    /// <remarks>
    /// The supported range is ±999 999 999 999.99, which every <see cref="int"/> value is within,
    /// so this overload does not throw.
    /// </remarks>
    public static AmountInWords InWords(
        this int amount)
        => new(amount);

    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount.</param>
    /// <remarks>
    /// The amount is validated here, so an invalid amount fails where it is passed in: it must be
    /// within ±999 999 999 999. The resulting builder's <see cref="AmountInWords.ToString"/> does
    /// not throw.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is outside ±999 999 999 999.</exception>
    public static AmountInWords InWords(
        this long amount)
        => new(amount);
}
