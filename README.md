# HelloDev Analytics

Reusable analytics abstractions and event data models for Unity projects. The package includes:

- `IAnalytics` and `AnalyticsManagerBase` for provider-independent analytics flows.
- Essential data and strongly typed tutorial event models.
- A Unity Services Analytics adapter with local consent storage.

## Installation

Add the package to Unity Package Manager with:

```text
https://github.com/joaovictoralencar/hellodev-analytics.git
```

For a released version, append the tag, for example `#v0.2.0`.

## Dependencies

- UniTask
- Unity Services Analytics

Consent state is stored locally with Unity `PlayerPrefs`. The package does not require a project save system, authentication system, locator system, scene system, or logging package.

## Project integration adapter

`Runtime/Analytics/Integration/UnityAnalyticsManager.cs` provides a Unity Services Analytics manager. Its assembly is disabled by default so projects can opt into the Unity Services adapter explicitly.

To compile that adapter, add the `HELLODEV_ANALYTICS_PROJECT_INTEGRATION` scripting define symbol. Set the player ID through the manager before initialization when a project-specific ID is available; otherwise the application identifier is used.
