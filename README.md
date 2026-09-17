# HelloDev Analytics

Reusable analytics abstractions and event data models for Unity projects. The package includes:

- `IAnalytics` and `AnalyticsManagerBase` for provider-independent analytics flows.
- Essential data and strongly typed tutorial event models.
- An optional Unity Services Analytics adapter for project integrations.

## Installation

Add the package to Unity Package Manager with:

```text
https://github.com/joaovictoralencar/hellodev-analytics.git
```

For a released version, append the tag, for example `#v0.1.0`.

## Dependencies

- UniTask
- HelloDev Utils
- Unity Services Analytics

## Project integration adapter

`Runtime/Analytics/Integration/UnityAnalyticsManager.cs` is an optional project integration adapter. Its assembly is disabled by default because it depends on project-specific connection, save, locator, and logging assemblies.

To compile that adapter in a project, add the `HELLODEV_ANALYTICS_PROJECT_INTEGRATION` scripting define symbol and provide those project dependencies.
