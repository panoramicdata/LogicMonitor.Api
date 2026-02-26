---
name: audit-event-unhandled-logs
description: "Use when adding ToAuditEvent support for unhandled event log messages in LogicMonitor.Api.Test/EventLogs/UnhandledLogs, including creating characterization tests, regex fixes, and rerunning AuditEvent tests."
---

# Audit Event Unhandled Logs Workflow

Use this workflow to process unhandled messages from `LogicMonitor.Api.Test/EventLogs/UnhandledLogs`.

## Assets

- `assets/Get-UnhandledLogMessages.ps1`: parses the unhandled log files into clean message text blocks.

## Workflow

1. Extract candidate messages:
   - `pwsh ./.github/skills/audit-event-unhandled-logs/assets/Get-UnhandledLogMessages.ps1 -RootPath ./LogicMonitor.Api.Test/EventLogs/UnhandledLogs -First 20`
2. Pick the first message that is not currently parsed by `LogItem.ToAuditEvent()`.
3. Add or update a focused unit test in `LogicMonitor.Api.Test/EventLogs/AuditEventTests.cs` (or adjacent test file):
   - First characterize the current behavior.
   - Then update test expectations to the desired parsed result.
4. Implement parsing fix in `LogicMonitor.Api/Extensions/LogItemExtensions.cs`:
   - Prefer a targeted regex entry and required action overrides.
5. Validate:
   - Targeted test(s) for the new message.
   - `dotnet test .\\LogicMonitor.Api.Test\\LogicMonitor.Api.Test.csproj --filter "FullyQualifiedName~AuditEventTests"`

## Notes

- Keep regexes minimal and specific to avoid regressions.
- Reuse existing action/entity conventions already used in `AuditEventTests`.
- Repeat the cycle until all desired unhandled messages are covered.
