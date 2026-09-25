# FluentBgWords

[![CI](https://github.com/aleksandarMilev/fluent-bg-words/actions/workflows/ci.yml/badge.svg)](https://github.com/aleksandarMilev/fluent-bg-words/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/FluentBgWords.svg)](https://www.nuget.org/packages/FluentBgWords)

Fluent .NET API for writing amounts in Bulgarian words (**сума словом**) for invoices,
contracts, receipts and payment orders. Supports euro (EUR) and Bulgarian leva (BGN)
for legacy data, with correct grammatical gender and count forms.

## Install

```bash
dotnet add package FluentBgWords
```

Targets `net8.0` and `net10.0` (works on .NET 8 and later).

## Quick start

```csharp
using FluentBgWords;

1234.56m.InWords().ToString();
// хиляда двеста тридесет и четири евро и петдесет и шест цента

1234.56m.InWords().AsEurWithEurocents().ToString();
// хиляда двеста тридесет и четири евро и петдесет и шест евроцента

21.01m.InWords().AsBgn().ToString();
// двадесет и един лева и една стотинка

167.42m.InWords().AsBgn().WithSubunitsAsDigits().Abbreviated().Capitalized().ToString();
// Сто шестдесет и седем лв. и 42 ст.

5.InWords().AsBgn().ToString();
// пет лева
```

## API

| Method                   | Effect                                                        | Example output             |
| ------------------------ | ------------------------------------------------------------- | -------------------------- |
| `InWords()`              | Starts the chain (`decimal`, `int`, `long`). Defaults to EUR. | `пет евро`                 |
| `AsEur()`                | Euro with "цент"                                              | `две евро и два цента`     |
| `AsEurWithEurocents()`   | Euro with "евроцент"                                          | `две евро и два евроцента` |
| `AsBgn()`                | Bulgarian leva                                                | `два лева и две стотинки`  |
| `As(Currency)`           | Custom currency                                               | `два долара и един цент`   |
| `WithSubunitsAsDigits()` | Subunits as digits                                            | `пет лева и 42 стотинки`   |
| `Abbreviated()`          | Currency abbreviations                                        | `пет лв. и 42 ст.`         |
| `Capitalized()`          | Capitalizes the first letter                                  | `Пет лева`                 |

The builder is **immutable**: every method returns a new instance, so configurations can be
shared and reused safely.

### Custom currencies

```csharp
var usd = new Currency(
    new Unit("долар", "долара", Gender.Masculine),
    new Unit("цент", "цента", Gender.Masculine),
    "$",
    "ц.");

2.01m.InWords().As(usd).ToString();
// два долара и един цент
```

`Unit` takes the singular form (used for exactly 1), the count form (used for every other
amount, including 21, 101…) and the grammatical gender.

## Supported currencies

| Currency                    | Major           | Minor                | Abbreviations |
| --------------------------- | --------------- | -------------------- | ------------- |
| `Currency.Eur`              | евро (neuter)   | цент (masculine)     | е. / ц.       |
| `Currency.EurWithEurocents` | евро (neuter)   | евроцент (masculine) | е. / е.ц.     |
| `Currency.Bgn`              | лев (masculine) | стотинка (feminine)  | лв. / ст.     |

## Behavior

- **At most 2 decimal places.** `1.234m` throws `ArgumentException`. Rounding money is the
  caller's decision, so the library never rounds silently. `1.230m` is accepted.
- **Range:** ±999 999 999 999.99. Outside it, `ArgumentOutOfRangeException` is thrown.
- **Negative amounts** are prefixed with "минус".
- **Zero subunits are omitted:** `5m` → "пет евро", not "пет евро и нула цента".
- **No currency conversion.** The library writes amounts; converting BGN to EUR
  (at 1.95583) is the caller's responsibility.

## Grammar rules and sources

- **Gender** decides "един/една/едно" and "два/две": "един лев", "една стотинка", "едно евро".
- **Count form** follows every number except exactly 1: "два лева", "двадесет и един лева".
- **"и"** goes before the last word of a group ("сто двадесет и един") and between groups when
  the last group is a single word ("хиляда и сто", but "хиляда сто и двадесет").
- **Thousands ending in 1** use "хиляди": "двадесет и една хиляди".

Currency terms and abbreviations follow:

- [Закон за въвеждане на еврото в Република България](https://www.tita.bg/laws/899), art. 4:
  one euro is divided into one hundred cents
- [evroto.bg: guidance on abbreviations](https://evroto.bg/bg/news/399-nasoki-za-izpisvane-na-sakrashteniyata-na-evro-i-evrotsent-):
  "е.", "ц.", "е.ц.", "ст."

Found a case that reads wrong? [Open an issue](https://github.com/aleksandarMilev/fluent-bg-words/issues)
with the amount, the output, and the correct form.

## License

[MIT](https://github.com/aleksandarMilev/fluent-bg-words/blob/master/LICENSE)
