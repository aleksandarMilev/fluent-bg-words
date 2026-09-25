namespace FluentBgWords;

/// <summary>Entry points of the fluent API.</summary>
public static class AmountWordsExtensions
{
    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount; at most 2 decimal places.</param>
    public static AmountInWords InWords(
        this decimal amount)
        => new(amount);

    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount.</param>
    public static AmountInWords InWords(
        this int amount)
        => new(amount);

    /// <summary>Starts writing <paramref name="amount"/> in Bulgarian words. Defaults to euro.</summary>
    /// <param name="amount">The amount.</param>
    public static AmountInWords InWords(
        this long amount)
        => new(amount);
}
