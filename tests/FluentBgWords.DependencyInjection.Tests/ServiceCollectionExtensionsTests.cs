namespace FluentBgWords.DependencyInjection.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public class ServiceCollectionExtensionsTests
{
    private const string NullCurrencyMessage =
        "AmountWordsOptions.Currency must not be null. Set it to a CurrencyDefinition, such as CurrencyDefinition.Eur.";

    [Fact]
    public void AddFluentBgWords_NoConfiguration_WritesEuro()
    {
        using var provider = Build(new ServiceCollection().AddFluentBgWords());

        Assert.Equal(
            "пет евро",
            provider.GetRequiredService<IAmountWordsFormatter>().Format(5m));
    }

    [Fact]
    public void AddFluentBgWords_WithConfiguration_AppliesOptions()
    {
        using var provider = Build(new ServiceCollection().AddFluentBgWords(static options =>
        {
            options.Currency = CurrencyDefinition.Bgn;
            options.SubunitsAsDigits = true;
            options.Abbreviated = true;
            options.Capitalized = true;
        }));

        Assert.Equal(
            "Сто шестдесет и седем лв. и 42 ст.",
            provider.GetRequiredService<IAmountWordsFormatter>().Format(167.42m));
    }

    [Fact]
    public void AddFluentBgWords_ResolvedTwice_ReturnsSameInstance()
    {
        using var provider = Build(new ServiceCollection().AddFluentBgWords());
        using var scope = provider.CreateScope();

        var first = provider.GetRequiredService<IAmountWordsFormatter>();

        Assert.Same(first, provider.GetRequiredService<IAmountWordsFormatter>());
        Assert.Same(first, scope.ServiceProvider.GetRequiredService<IAmountWordsFormatter>());
    }

    [Fact]
    public void AddFluentBgWords_FormatterRegisteredBefore_KeepsUserRegistration()
    {
        var services = new ServiceCollection()
            .AddSingleton<IAmountWordsFormatter, FixedFormatter>()
            .AddFluentBgWords();

        Assert.Single(
            services,
            static d => d.ServiceType == typeof(IAmountWordsFormatter));

        using var provider = Build(services);

        Assert.IsType<FixedFormatter>(provider.GetRequiredService<IAmountWordsFormatter>());
    }

    [Fact]
    public void AddFluentBgWords_CalledTwice_RegistersOneFormatterAndAppliesBothConfigurations()
    {
        var services = new ServiceCollection()
            .AddFluentBgWords(static options => options.Currency = CurrencyDefinition.Bgn)
            .AddFluentBgWords(static options => options.Abbreviated = true);

        Assert.Single(
            services,
            static d => d.ServiceType == typeof(IAmountWordsFormatter));

        using (var provider = Build(services))
        {
            Assert.Equal(
                5.42m.InWords().AsBgn().Abbreviated().ToString(),
                provider.GetRequiredService<IAmountWordsFormatter>().Format(5.42m));
        }

        var invalid = new ServiceCollection()
            .AddFluentBgWords(static options => options.Currency = null!)
            .AddFluentBgWords();

        using var invalidProvider = Build(invalid);

        var exception = Assert.Throws<OptionsValidationException>(
            () => invalidProvider.GetRequiredService<IAmountWordsFormatter>());

        Assert.Equal(
            NullCurrencyMessage,
            Assert.Single(exception.Failures));
    }

    [Fact]
    public void AddFluentBgWords_NullCurrency_ThrowsOptionsValidationException()
    {
        using var provider = Build(new ServiceCollection().AddFluentBgWords(
            static options => options.Currency = null!));

        var exception = Assert.Throws<OptionsValidationException>(
            () => provider.GetRequiredService<IAmountWordsFormatter>());

        Assert.Equal(typeof(AmountWordsOptions), exception.OptionsType);
        Assert.Contains(NullCurrencyMessage, exception.Failures);
    }

    [Fact]
    public async Task AddFluentBgWords_NullCurrencyInHost_FailsAtStartup()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddFluentBgWords(static options => options.Currency = null!);

        using var host = builder.Build();

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(
            () => host.StartAsync(TestContext.Current.CancellationToken));

        Assert.Equal(
            NullCurrencyMessage,
            Assert.Single(exception.Failures));
    }

    [Fact]
    public void AddFluentBgWords_NamedOptionsWithNullCurrency_AreNotValidated()
    {
        var services = new ServiceCollection().AddFluentBgWords();
        services.Configure<AmountWordsOptions>(
            "x",
            static options => options.Currency = null!);

        using var provider = Build(services);

        Assert.Null(provider
            .GetRequiredService<IOptionsMonitor<AmountWordsOptions>>()
            .Get("x")
            .Currency);

        Assert.Equal(
            "пет евро",
            provider.GetRequiredService<IAmountWordsFormatter>().Format(5m));
    }

    [Fact]
    public void AddFluentBgWords_NullServices_ThrowsArgumentNullException()
    {
        IServiceCollection services = null!;

        var withoutConfigure = Assert.Throws<ArgumentNullException>(
            () => services.AddFluentBgWords());

        var withConfigure = Assert.Throws<ArgumentNullException>(
            () => services.AddFluentBgWords(static _ => { }));

        Assert.Equal("services", withoutConfigure.ParamName);
        Assert.Equal("services", withConfigure.ParamName);
    }

    [Fact]
    public void AddFluentBgWords_NullConfigure_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new ServiceCollection().AddFluentBgWords(null!));

        Assert.Equal("configure", exception.ParamName);
    }

    [Fact]
    public void AddFluentBgWords_ReturnsSameServiceCollection()
    {
        var services = new ServiceCollection();

        Assert.Same(services, services.AddFluentBgWords());
        Assert.Same(services, services.AddFluentBgWords(static _ => { }));
    }

    private static ServiceProvider Build(IServiceCollection services)
        => services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        });

    private sealed class FixedFormatter : IAmountWordsFormatter
    {
        public string Format(decimal amount)
            => "fixed";
    }
}
