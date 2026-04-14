# com.mutant.vr

这一版是按你当前包结构做的小步重构版，不推翻现有设计，重点解决一件事：

**把“场景装配”和“生命周期驱动”拆开。**

## 重构后的职责

- `Bootstrap/MutantVrInstaller`
  - 只负责收集场景引用、构建 RuntimeContext、构建 Runtime
  - 不再默认用 `Awake / Update / OnDestroy` 自己驱动生命周期

- `Bootstrap/MutantVrStandaloneHost`
  - 仅用于独立场景 / Demo 模式
  - 用 MonoBehaviour 驱动 `Install / Tick / Shutdown`

- `Core/MutantVrRuntime`
  - 真正的 VR 运行时内核
  - 管理 `Install / Tick / Shutdown`
  - 驱动 `IMutantVrPlatformAdapter`

- `Core/MutantVrContext`
  - 保存 Settings / RigReferences / PlatformAdapter / Diagnostics / ServiceRegistry / State

- `Contracts/IMutantVrPlatformAdapter`
  - 平台适配抽象，不绑定某个具体 SDK

- `Rig/*`
  - Rig 引用收集

- `Diagnostics/*`
  - 日志与诊断出口抽象

## 推荐运行模式

### 1. Standalone / Demo 模式
场景里挂：

- `MutantVrInstaller`
- `MutantVrStandaloneHost`
- 你的某个 `IMutantVrPlatformAdapter` 实现

### 2. Mutant Core 模式
由外部 Core 宿主统一驱动：

- `Install`
- `Tick(deltaTime)`
- `Shutdown`

这时不要再挂 `MutantVrStandaloneHost`。

## 迁移说明

### 删除旧文件
- `Runtime/Bootstrap/MutantVrModule.cs`

### 新文件替代
- `Runtime/Core/MutantVrRuntime.cs`

### CoreBridge 示例
建议从：
- `Runtime/CoreBridge/MutantVrCoreInstallExample.cs`

迁到：
- `Samples~/CoreBridge/MutantVrCoreInstallExample.cs`

## 设计目标

- 保留 `Installer + Context + Adapter + Rig` 这条主线
- 不让主包强依赖 `com.mutant.core`
- 让 `com.mutant.vr` 既能独立跑 Demo，也能挂到 Mutant Core 下面
