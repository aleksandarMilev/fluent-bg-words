namespace Microsoft.Extensions.DependencyInjection;

using FluentBgWords;
using FluentBgWords.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

/// <summary>Registers FluentBgWords services in an <see cref="IServiceCollection"/>.</summary>
public static class FluentBgWordsServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IAmountWordsFormatter"/> with the default settings: euro, no other options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns><paramref name="services"/>, for chaining.</returns>
    /// <remarks>
    /// The formatter is a singleton. Its <see cref="AmountWordsOptions"/> are read once, when the
    /// formatter is first created; later changes to the options have no effect on it. An
    /// <see cref="IAmountWordsFormatter"/> registered before this call is kept, and calling this
    /// method more than once registers a single formatter.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddFluentBgWords(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return AddFormatter(
            services,
            configure: null);
    }

    /// <summary>
    /// Registers <see cref="IAmountWordsFormatter"/> with settings set by <paramref name="configure"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Sets the currency and formatting options.</param>
    /// <returns><paramref name="services"/>, for chaining.</returns>
    /// <remarks>
    /// The formatter is a singleton. Its <see cref="AmountWordsOptions"/> are read once, when the
    /// formatter is first created; later changes to the options have no effect on it. An
    /// <see cref="IAmountWordsFormatter"/> registered before this call is kept. Calling this
    /// method more than once registers a single formatter, and every <paramref name="configure"/>
    /// delegate is applied, in order. A <see langword="null"/>
    /// <see cref="AmountWordsOptions.Currency"/> fails options validation: at startup in a host,
    /// otherwise when the formatter is first resolved, with an <see cref="OptionsValidationException"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> or <paramref name="configure"/> is <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddFluentBgWords(
        this IServiceCollection services,
        Action<AmountWordsOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        return AddFormatter(
            services,
            configure);
    }

    private static IServiceCollection AddFormatter(
        IServiceCollection services,
        Action<AmountWordsOptions>? configure)
    {
        var options = services.AddOptions<AmountWordsOptions>();

        if (configure is not null)
        {
            options.Configure(configure);
        }

        options.ValidateOnStart();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IValidateOptions<AmountWordsOptions>, AmountWordsOptionsValidator>());

        services.TryAddSingleton<IAmountWordsFormatter>(
            static provider => new AmountWordsFormatter(
                provider.GetRequiredService<IOptions<AmountWordsOptions>>().Value));

        return services;
    }
}
