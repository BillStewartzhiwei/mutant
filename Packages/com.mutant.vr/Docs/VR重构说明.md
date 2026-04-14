# com.mutant.vr 重构说明

## 目标

在保留你当前 `Installer + Context + Adapter + Rig` 设计主线的前提下，
把 `com.mutant.vr` 从“Installer 自驱的可运行骨架”收敛成：

- 可独立运行的 VR Runtime 包
- 可被 Mutant Core 接管生命周期的 VR 能力包

## 关键调整

### 1. Installer 不再自动驱动生命周期
`MutantVrInstaller` 现在只负责：
- 收集 Settings
- 收集 RigRoot
- 收集 PlatformAdapter
- 构建 `MutantVrContext`
- 构建 `MutantVrRuntime`

### 2. 新增 StandaloneHost
`MutantVrStandaloneHost` 用于独立 Demo 场景。
只有在你明确想用 MonoBehaviour 模式驱动时，才挂这个组件。

### 3. MutantVrModule -> MutantVrRuntime
旧的 `Runtime/Bootstrap/MutantVrModule.cs` 建议删除，
改用 `Runtime/Core/MutantVrRuntime.cs` 作为正式运行时内核。

### 4. Settings 移除宿主策略字段
以下字段不再属于 VR Runtime 配置：
- `AutoInstallOnAwake`
- `AutoTickInUpdate`
- `AutoShutdownOnDestroy`

### 5. CoreBridge 作为示例迁到 Samples
`MutantVrCoreInstallExample` 仍然保留，但建议从 Runtime 主路径迁到 `Samples~/CoreBridge/`。

## 你怎么用

### Standalone / Demo 模式
场景挂：
- `MutantVrInstaller`
- `MutantVrStandaloneHost`
- 某个 `IMutantVrPlatformAdapter` 实现

### Mutant Core 模式
由外部 Core 调：
- `Install()`
- `Tick(deltaTime)`
- `Shutdown()`

不再挂 `MutantVrStandaloneHost`。
