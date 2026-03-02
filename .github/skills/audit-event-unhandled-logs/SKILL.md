---
name: audit-event-unhandled-logs
description: "Use when adding ToAuditEvent support for unhandled event log messages in LogicMonitor.Api.Test/EventLogs/UnhandledLogs, including creating characterization tests, regex fixes, and rerunning AuditEvent tests."
---

# Audit Event Unhandled Logs Workflow

Use this workflow to process unhandled messages from `LogicMonitor.Api.Test/EventLogs/UnhandledLogs`.

## Assets

- `assets/Get-UnhandledLogMessages.ps1`: parses the unhandled log files into clean message text blocks.
- `assets/Scan-UnhandledLogMessages.cs`: runs each unhandled message through `LogItem.ToAuditEvent()` and outputs only unmatched/errored messages.

## Scanner Quick Start

- Default scan (recommended):
   - `dotnet run ./.github/skills/audit-event-unhandled-logs/assets/Scan-UnhandledLogMessages.cs -- --root ./LogicMonitor.Api.Test/EventLogs/UnhandledLogs --first 20 --progress-every 25 --regex-timeout-seconds 5`
- Verbose diagnostics (when it appears stalled):
   - `dotnet run ./.github/skills/audit-event-unhandled-logs/assets/Scan-UnhandledLogMessages.cs -- --root ./LogicMonitor.Api.Test/EventLogs/UnhandledLogs --first 20 --progress-every 10 --regex-timeout-seconds 5 --log-each-message`

## Scanner Output Interpretation

- `TIMEOUT` lines indicate a very large/pathological message; these are counted under `Errored` and scanning continues.
- `Progress` lines provide processed/handled/unhandled/errored counts with elapsed time.
- Ranked entries like `[1] count=...` are unmatched patterns, sorted by frequency; start with the highest count.
- Summary lines (`Total messages`, `Handled`, `Unhandled`, `Errored`, `Unique unmatched patterns`) are the key health metrics.

## Scanner Options

- `--root <path>`: folder containing unhandled log files.
- `--first <n>`: number of unmatched patterns to print.
- `--progress-every <n>`: progress heartbeat interval (in messages processed).
- `--regex-timeout-seconds <n>`: per-message regex timeout safeguard.
- `--log-each-message`: prints a timestamped line before each message parse.

## Workflow

1. Extract candidate messages:
   - `dotnet run ./.github/skills/audit-event-unhandled-logs/assets/Scan-UnhandledLogMessages.cs -- --root ./LogicMonitor.Api.Test/EventLogs/UnhandledLogs --first 20 --progress-every 25 --regex-timeout-seconds 5`
   - If it appears slow/stuck, add per-message tracing: `--log-each-message`
   - Optional (raw view): `pwsh ./.github/skills/audit-event-unhandled-logs/assets/Get-UnhandledLogMessages.ps1 -RootPath ./LogicMonitor.Api.Test/EventLogs/UnhandledLogs -First 20`
2. Pick the first message that is not currently parsed by `LogItem.ToAuditEvent()`.
3. Add or update a focused unit test in `LogicMonitor.Api.Test/EventLogs/AuditEventTests.cs` (or adjacent test file):
   - First characterize the current behavior.
   - Then update test expectations to the desired parsed result.
4. Implement parsing fix in `LogicMonitor.Api/Extensions/LogItemExtensions.cs`:
   - Prefer a targeted regex entry and required action overrides.
5. Validate:
   - Build first to ensure test discovery/count reflects compiled sources:
     - `dotnet build .\\LogicMonitor.Api.Test\\LogicMonitor.Api.Test.csproj`
   - Targeted test(s) for the new message.
   - `dotnet test .\\LogicMonitor.Api.Test\\LogicMonitor.Api.Test.csproj --filter "FullyQualifiedName~AuditEventTests"`

## Notes

- After adding each new validation (test/assertion), pause and wait for user input before proceeding to the next unhandled message.
- Keep regexes minimal and specific to avoid regressions.
- Reuse existing action/entity conventions already used in `AuditEventTests`.
- Always build before running unit tests so `AuditEventTests` counts are accurate for the current code.
- Repeat the cycle until all desired unhandled messages are covered.
