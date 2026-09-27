# Contributing to FluentBgWords

Thanks for helping. This guide describes how this repository actually works. By taking part,
you agree to follow the [Code of Conduct](CODE_OF_CONDUCT.md).

## Reporting problems

Every issue starts from one of two forms (blank issues are turned off):

- **Grammar report**: an amount that reads wrong in Bulgarian. Give the exact amount as written
  in code, the currency, the fluent chain you ran, the actual output and the expected output.
  A source for the correct form (a dictionary entry, a grammar rule or an official document)
  helps a lot. With these details, the case can be turned into a test.
- **Bug report**: anything else, such as an exception, a crash or a packaging problem. Give the
  affected package (`FluentBgWords`, `FluentBgWords.DependencyInjection` or both), its version,
  the .NET version, a minimal code sample, and the expected and actual behavior, including the
  full exception and stack trace if there is one.

Don't report security vulnerabilities in public issues. See [SECURITY.md](SECURITY.md).

## Prerequisites

- **.NET SDK 10.0.401 or later.** `global.json` pins `10.0.401` with `rollForward:
  latestFeature`, so any later 10.0 SDK works (a later patch or feature band, such as 10.0.5xx),
  but not an SDK from a different major or minor version.
- **The .NET 8 runtime.** The libraries target `net8.0` and `net10.0`, and the tests run on
  both. The .NET 10 SDK includes only the .NET 10 runtime, so install the .NET 8 runtime too.

## Build and test

From the repository root:

```bash
dotnet build -c Release
dotnet test -c Release
```

The solution (`FluentBgWords.slnx`) has two libraries, `src/FluentBgWords` and
`src/FluentBgWords.DependencyInjection`, and a test project for each. A passing test run
reports four runs, all green: `FluentBgWords.Tests` and
`FluentBgWords.DependencyInjection.Tests`, each on `net8.0` and `net10.0`.

Warnings are errors, and every public member needs XML documentation comments: a missing one
fails the build.

CI (`.github/workflows/ci.yml`) runs, in order: restore, `dotnet format --verify-no-changes`,
build, test, and pack with package validation.

## Code style

The style is defined in `.editorconfig` and checked in CI with:

```bash
dotnet format --verify-no-changes
```

The conventions that are easy to miss:

- `using` directives go **inside** the namespace, under the file-scoped `namespace …;` line,
  with `System` directives first.
- File-scoped namespaces (`namespace FluentBgWords;`).
- Instance members are always qualified with `this.` (fields, properties, methods and events).
- Encodings: `.cs`, `.csproj`, `.props`, `.targets` and `.xml` files are UTF-8 **with** BOM;
  `.json`, `.yml` and `.yaml` files are UTF-8 without BOM.
- LF line endings and a final newline in every file. C# is indented with 4 spaces; project,
  XML, JSON and YAML files with 2. `.gitattributes` makes git check out every text file with LF,
  so you don't need any special git settings (such as `core.autocrlf`), on Windows either.

**Known pitfall:** running `dotnet format` (without `--verify-no-changes`) on these
multi-targeted projects can leave `<<<<<<<` conflict markers in source files. Run
`dotnet build` right after it, and check `git diff` before committing.

The commit that applied this style to the whole codebase is listed in `.git-blame-ignore-revs`.
To make `git blame` skip it, run this once in your clone:

```bash
git config blame.ignoreRevsFile .git-blame-ignore-revs
```

## Tests are the specification

The tests pin the exact Bulgarian text the library writes. **Never change an expected output
to make a test pass.**

- A change to the Bulgarian output is a grammar change. It needs a source (a dictionary entry,
  a grammar rule or an official document), and the tests change in the same pull request as
  the code.
- New behavior and bug fixes come with tests. Tests are grouped by area, one class per area
  (for example `NumberToWordsTests`, `FormatMatrixTests`, `PublicApiEdgeTests`), and most are
  named `Method_Scenario_ExpectedResult`.
- Every code example in `README.md` is pinned by a test, in `ReadmeExamplesTests` or
  `FluentApiTests`; in `ReadmeExamplesTests`, the comment above each test names the README
  section. If you change an example, change its test.
- Some expected values are marked `// NEEDS NATIVE VERIFICATION, see issue #7.` They pin the
  current output for cases that haven't been checked against a grammar source yet (the "и"
  between three or more number groups, and spelled-out numbers next to abbreviations). They're
  tracked in [issue #7](https://github.com/aleksandarMilev/fluent-bg-words/issues/7). Change
  them only with a source, and mention it in that issue.

## Public API changes

Package validation runs when a package is packed with any version that doesn't start with
`0.0.0-`. It checks that the `net8.0` and `net10.0` builds match and, for a package that has a
`PackageValidationBaselineVersion` in its project file, compares the public API with that
released version. CI packs both packages on every pull request, so an unintended breaking
change fails the `build` check.

Local packs default to version `0.0.0-dev`, which skips the comparison with the released
version. To run it locally, pack with an explicit version into a directory outside the
repository:

```bash
dotnet pack src/FluentBgWords -c Release -p:Version=0.2.1-local -o <temp dir>
```

If a breaking change is **intended**, record it:

1. Generate the suppression file:

   ```bash
   dotnet pack src/FluentBgWords -c Release -p:Version=0.2.1-local \
     -p:ApiCompatGenerateSuppressionFile=true -o <temp dir>
   ```

   This writes `CompatibilitySuppressions.xml` in the project folder (here
   `src/FluentBgWords/CompatibilitySuppressions.xml`). The SDK writes it with CRLF line
   endings; convert it to LF.
2. Review every entry: each one must match a change you meant to make.
3. Add a changelog entry marked **BREAKING**, with migration notes (old name → new name, what
   callers have to change).

## Changelog

`CHANGELOG.md` follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Every
user-visible change gets an entry under `## [Unreleased]`, in the matching section
(`### Added`, `### Changed`, `### Fixed`). Write it for users: what changed and what they need
to do, not how the code works. Don't add a version heading; that happens at release.

## Pull requests

- `master` is protected. Every change goes through a pull request, and the `build` check (the
  single job in `ci.yml`) must pass.
- Pull requests are merged with rebase, so history stays linear and each of your commits lands
  on `master` as it is. Keep commits focused, and make each one build and pass the tests.
- Commit messages start with a lowercase type and a colon, followed by a short summary in the
  imperative, for example `fix: reject undefined grammatical gender values`. The types used
  are `feat`, `fix`, `docs`, `test`, `ci`, `build`, `refactor`, `style` and `chore`. A `!`
  after the type marks a breaking change, for example
  `feat!: validate amounts in InWords() instead of ToString()`. This is the style of the
  existing history, not a tool-enforced rule.
- The pull request template has a short checklist. Please fill it in.

## Releases

Releases are done by the maintainer. Both packages, `FluentBgWords` and
`FluentBgWords.DependencyInjection`, are released together with the same version, which comes
from the release tag. Don't bump versions in your pull request: the project files keep
`0.0.0-dev`.
