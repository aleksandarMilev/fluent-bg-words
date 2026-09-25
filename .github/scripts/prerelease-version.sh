#!/usr/bin/env bash
# Prints a prerelease version one patch above the latest v* tag:
#   bash .github/scripts/prerelease-version.sh <label> <run number>
#   latest tag v0.1.1, label "ci", run 7 -> 0.1.2-ci.7
# The checkout needs tags (actions/checkout with fetch-depth: 0).
set -euo pipefail

if [[ $# -ne 2 ]]; then
  echo "::error::Usage: prerelease-version.sh <label> <run number>" >&2
  exit 2
fi

label="$1"
run_number="$2"

if [[ ! "$label" =~ ^[0-9A-Za-z-]+$ ]]; then
  echo "::error::Label '$label' must contain only letters, digits and hyphens." >&2
  exit 2
fi

if [[ ! "$run_number" =~ ^[0-9]+$ ]]; then
  echo "::error::Run number '$run_number' must be a non-negative integer." >&2
  exit 2
fi

if ! latest_tag="$(git describe --tags --abbrev=0 --match 'v*' 2>/dev/null)"; then
  echo "::error::No v* release tag is reachable from HEAD. The prerelease version is derived from the latest release tag; check out with fetch-depth: 0." >&2
  exit 1
fi

if [[ ! "$latest_tag" =~ ^v([0-9]+)\.([0-9]+)\.([0-9]+)(-.*)?$ ]]; then
  echo "::error::Latest release tag '$latest_tag' is not v<major>.<minor>.<patch>; cannot derive a prerelease version." >&2
  exit 1
fi

echo "Latest release tag: $latest_tag" >&2
echo "${BASH_REMATCH[1]}.${BASH_REMATCH[2]}.$((BASH_REMATCH[3] + 1))-$label.$run_number"
