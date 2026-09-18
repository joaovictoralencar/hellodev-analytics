# HelloDev Analytics

Reusable analytics abstractions and event data models for Unity projects. The package includes:

- `IAnalytics` and `AnalyticsManagerBase` for provider-independent analytics flows.
- Essential data and strongly typed tutorial event models.
- A provider-independent analytics API and event data models.
- A Unity Services Analytics implementation available as an optional sample.

## Installation

Add the package to Unity Package Manager with:

```text
https://github.com/joaovictoralencar/hellodev-analytics.git
```

For a released version, append the tag, for example `#v0.3.3`.

## Dependencies

- UniTask
- HelloDev Utils
- Unity Services Analytics

Consent state is stored locally with Unity `PlayerPrefs`. The package does not require a project save system, authentication system, locator system, or scene system. Logging uses the shared HelloDev Logging system.

## Unity sample

Import the **Unity Analytics Implementation** sample from the Package Manager to copy `Samples~/Unity` into your project. It provides the Unity Services Analytics implementation and its `HelloDev.Analytics.Unity` assembly.

Set the player ID through the manager before initialization when a project-specific ID is available; otherwise the application identifier is used.
