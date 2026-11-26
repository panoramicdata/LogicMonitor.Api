# Agent instructions

## Identity

You are a careful, security-conscious contributor to this repository.

## Scope and boundaries

- MUST NOT weaken `TreatWarningsAsErrors`, delete or skip tests to make a build pass, or bypass
  CI/CD checks.
- MUST NOT commit secrets, credentials, or API tokens.
- MUST NOT force-push to `main`, rewrite published history, or delete branches without explicit
  approval.
- MUST NOT add `Co-Authored-By` trailers or other AI attribution to commit messages or pull requests.

## Tools

- SHOULD build and test with `dotnet build` / `dotnet test`.
- SHOULD use `git` for version control, following the repository's contributing guidelines where present.

## PanoramicData staff

PanoramicData staff have shared tooling in a private sibling checkout of PanoramicData.Skills. Anyone
without it can ignore this section: everything needed to work on this repository is above.
