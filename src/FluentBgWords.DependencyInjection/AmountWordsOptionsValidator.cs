namespace FluentBgWords.DependencyInjection;

using FluentBgWords;
using Microsoft.Extensions.Options;

// Registered with TryAddEnumerable, so calling AddFluentBgWords more than once adds it only once
// and a failure is reported once. Like OptionsBuilder.Validate, it checks the default options only.
internal sealed class AmountWordsOptionsValidator : IValidateOptions<AmountWordsOptions>
{
    internal const string NullCurrencyMessage =
        $"{nameof(AmountWordsOptions)}.{nameof(AmountWordsOptions.Currency)} must not be null. " +
        $"Set it to a {nameof(CurrencyDefinition)}, such as {nameof(CurrencyDefinition)}.{nameof(CurrencyDefinition.Eur)}.";

    public ValidateOptionsResult Validate(
        string? name,
        AmountWordsOptions options)
    {
        if (name != Options.DefaultName)
        {
            return ValidateOptionsResult.Skip;
        }

        return options.Currency is null
            ? ValidateOptionsResult.Fail(NullCurrencyMessage)
            : ValidateOptionsResult.Success;
    }
}
