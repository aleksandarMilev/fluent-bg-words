# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- README: the validation rules for custom currencies, and how to handle nullable amounts
  with `amount?.InWords()`.
- XML docs: `InWords()` now explains that the amount is validated by `ToString()` and gives
  the supported range; `As()` documents its `ArgumentNullException`.

### Changed

- **Custom `Unit` and `Currency` values are now validated**, both when they are created and in
  `with` expressions. Values that were previously accepted now throw:
  - a `null` unit name, abbreviation, `Major` or `Minor`: `ArgumentNullException`;
  - an empty, whitespace-only, or padded (leading or trailing whitespace) unit name or
    abbreviation: `ArgumentException`;
  - an undefined `Gender` value, such as `(Gender)42`: `ArgumentOutOfRangeException`.

  Before, these values produced malformed text (extra spaces), a `NullReferenceException`
  from `ToString()`, or silently used the neuter forms.
- The package description now says what the library does: it writes monetary amounts, not
  arbitrary numbers.

### Fixed

- README: the `Abbreviated()` example in the API table showed output that also needs
  `WithSubunitsAsDigits()`. It now shows what `Abbreviated()` alone produces.
- README: the rule for placing "и" between groups is described correctly: it depends on
  whether the last group's number is a single word ("един милион и двеста хиляди").
- README and XML docs: exceptions for invalid amounts are documented as coming from
  `ToString()`, not from `InWords()`.
- README and XML docs: the builder is described as safe to branch from a partially
  configured amount, instead of as a reusable configuration.

## [0.1.1] - 2026-09-25

### Fixed

- README: the supported target frameworks are `net8.0` and `net10.0`. The 0.1.0 README
  wrongly listed `netstandard2.0`.

## [0.1.0] - 2026-09-25

### Added

- `InWords()` on `decimal`, `int` and `long` writes an amount in Bulgarian words
  (**сума словом**), with correct grammatical gender and count forms, up to
  ±999 999 999 999.99.
- Euro (the default, with "цент"), euro with "евроцент", and Bulgarian leva (BGN).
- Custom currencies with `Currency` and `Unit`.
- Formatting options: `WithSubunitsAsDigits()`, `Abbreviated()` and `Capitalized()`.
- Targets `net8.0` and `net10.0`.

[Unreleased]: https://github.com/aleksandarMilev/fluent-bg-words/compare/v0.1.1...HEAD
[0.1.1]: https://github.com/aleksandarMilev/fluent-bg-words/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/aleksandarMilev/fluent-bg-words/releases/tag/v0.1.0
