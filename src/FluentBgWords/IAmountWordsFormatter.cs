namespace FluentBgWords;

/// <summary>Writes amounts in Bulgarian words using settings configured once.</summary>
public interface IAmountWordsFormatter
{
    /// <summary>Returns <paramref name="amount"/> written in Bulgarian words.</summary>
    /// <param name="amount">The amount; at most 2 decimal places, within ±999 999 999 999.99.</param>
    /// <returns>The amount in words, e.g. "сто шестдесет и седем лева и 42 стотинки".</returns>
    /// <exception cref="ArgumentException"><paramref name="amount"/> has more than 2 decimal places.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is outside ±999 999 999 999.99.</exception>
    string Format(decimal amount);
}
