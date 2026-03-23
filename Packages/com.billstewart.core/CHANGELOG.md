# Changelog

All notable changes to Mutant Core will be documented in this file.

The format is based on Keep a Changelog.
This project adheres to Semantic Versioning.

---

## [1.0.0] - 2026-03-23

### Added
- Initial Mutant Core runtime kernel
- Module system with lifecycle (Init / Update / Dispose)
- Module dependency resolution via RequireAttribute
- ModuleAttribute for module discovery and ordering
- ModuleManager for module scanning and scheduling
- ModuleDependencyResolver for topological sorting
- BaseModule for simplified module implementation
- EventBus for decoupled communication
- Logger for unified logging
- ModuleBootstrap for automatic framework startup
- ModuleDriver for runtime update loop

### Added (Structure)
- Bootstrap directory for startup and runtime driver
- Module directory for lifecycle and dependency system
- Event directory for event communication
- Log directory for logging utilities
- Pattern directory for reusable design patterns
- Utility directory reserved for future core utilities

### Added (Patterns)
- Singleton base class
- MonoSingleton base class

### Changed
- Reorganized Core folder structure into Bootstrap / Module / Event / Log / Pattern
- Moved Logger into Log directory
- Moved ModuleDriver into Bootstrap directory
- Separated runtime kernel responsibilities

### Notes
- This version defines the stable Core runtime boundary
- UI, VR and other feature modules depend on this runtime