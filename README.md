# Sakumon Analytics

Reusable analytics abstractions and event data models for Unity projects. The package includes:

- `IAnalytics` and `AnalyticsManagerBase` for provider-independent analytics flows.
- Essential data and strongly typed tutorial event models.
- An optional Unity Services Analytics adapter for Sakumon project integrations.

## Installation

Add the package to Unity Package Manager with:

```text
https://github.com/joaovictoralencar/sakumon-analytics.git
```

For a released version, append the tag, for example `#v0.1.0`.

## Dependencies

- UniTask
- HelloDev Utils
- Unity Services Analytics

## Sakumon integration adapter

`Runtime/Analytics/Integration/UnityAnalyticsManager.cs` is retained for the Sakumon game integration. Its assembly is disabled by default because it depends on project-specific connection, save, locator, and logging assemblies.

To compile that adapter in the Sakumon project, add the `SAKUMON_ANALYTICS_PROJECT_INTEGRATION` scripting define symbol and provide those project dependencies.
