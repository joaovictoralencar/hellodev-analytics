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
- HelloDev Utils
- Unity Services Analytics

Consent state is stored locally with Unity `PlayerPrefs`. The package does not require a project save system, authentication system, locator system, or scene system. Logging uses the shared HelloDev Logging system.

## Unity implementation

`Runtime/Analytics/Unity/UnityAnalyticsManager.cs` provides the Unity Services Analytics implementation. The `HelloDev.Analytics.Unity` assembly is included automatically when the package is installed.

Set the player ID through the manager before initialization when a project-specific ID is available; otherwise the application identifier is used.
