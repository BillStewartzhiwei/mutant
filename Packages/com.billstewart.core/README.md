# Mutant Core Runtime

Mutant Core Runtime 是整个 Mutant 框架的运行时内核。

它负责：

- 框架启动
- 模块生命周期
- 模块依赖解析
- 事件通信
- 日志输出
- 基础设计模式支撑

它不负责：

- UI 逻辑
- VR 逻辑
- 网络业务
- 项目业务代码

---

## 目录结构

```text
Runtime/
├── Core.asmdef
├── README.md
│
├── Bootstrap/
│   ├── ModuleBootstrap.cs
│   └── ModuleDriver.cs
│
├── Event/
│   └── EventBus.cs
│
├── Log/
│   └── Logger.cs
│
├── Module/
│   ├── IModule.cs
│   ├── BaseModule.cs
│   ├── ModuleAttribute.cs
│   ├── RequireAttribute.cs
│   ├── ModuleInfo.cs
│   ├── ModuleDependencyResolver.cs
│   └── ModuleManager.cs
│
├── Pattern/
│   ├── Singleton.cs
│   └── MonoSingleton.cs
│
└── Utility/