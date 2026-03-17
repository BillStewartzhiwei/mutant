# Your Package Name - Documentation

## Overview
This package provides a template for creating Unity packages compatible with the Unity Package Manager (UPM).

## Getting Started
1. Import the package via Package Manager
2. Add `YourComponent` to any GameObject
3. Use `YourUtility` for static helper methods
4. Check the **Tools → YourPackage** menu for quick actions

## API Reference

### YourComponent
A MonoBehaviour that can be attached to GameObjects.
- `DoSomething()` - Logs a message with the component's configured text.

### YourUtility
A static utility class.
- `Log(string message)` - Logs a formatted message with package prefix.
- `Clamp01(float value)` - Clamps a float value between 0 and 1.
