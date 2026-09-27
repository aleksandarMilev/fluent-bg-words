# FluentBgWords.DependencyInjection

Registers the [FluentBgWords](https://www.nuget.org/packages/FluentBgWords) formatter with
`Microsoft.Extensions.DependencyInjection`, so ASP.NET Core and other .NET apps can configure
how amounts are written in Bulgarian words (**сума словом**) once and inject it anywhere.

```bash
dotnet add package FluentBgWords.DependencyInjection
```

```csharp
using FluentBgWords;

// Program.cs
builder.Services.AddFluentBgWords(options =>
{
    options.Currency = CurrencyDefinition.EurWithEurocents;
    options.SubunitsAsDigits = true;
    options.Abbreviated = true;
    options.Capitalized = true;
});

// anywhere
public class InvoiceService(IAmountWordsFormatter formatter)
{
    public string TotalInWords(decimal total) => formatter.Format(total);
}
```

`AddFluentBgWords()` without a delegate uses the defaults: euro, no other options.

- `IAmountWordsFormatter` is registered as a **singleton**. The options are read once, when the
  formatter is first created; later changes to them have no effect.
- An `IAmountWordsFormatter` you registered before calling `AddFluentBgWords` is kept.
- Calling `AddFluentBgWords` more than once registers one formatter; the configure delegates
  are applied in order.
- A `null` `Currency` fails options validation, at startup in apps that use the .NET Generic
  Host.

For the fluent API, the supported currencies and the formatting rules, see the
[FluentBgWords README](https://github.com/aleksandarMilev/fluent-bg-words/blob/master/README.md).
