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
│   ├── IEvent.cs
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
```

---

## 事件系统使用示例

```csharp
using Bill.Mutant.Core;
using UnityEngine;

public readonly struct PlayerDamagedEvent : IEvent
{
    public readonly int CurrentHp;

    public PlayerDamagedEvent(int currentHp)
    {
        CurrentHp = currentHp;
    }
}

public sealed class EventDemo : MonoBehaviour
{
    private System.IDisposable _subscription;

    private void OnEnable()
    {
        // priority 越大越早执行
        _subscription = EventBus.Subscribe<PlayerDamagedEvent>(OnPlayerDamaged, priority: 100);

        // 只触发一次
        EventBus.SubscribeOnce<PlayerDamagedEvent>(evt =>
        {
            Debug.Log($"[Once] first damage received, hp: {evt.CurrentHp}");
        });
    }

    private void OnDisable()
    {
        // 推荐：在生命周期结束时释放订阅
        _subscription?.Dispose();
    }

    public void SimulateDamage(int hp)
    {
        EventBus.Publish(new PlayerDamagedEvent(hp));
    }

    private void OnPlayerDamaged(PlayerDamagedEvent evt)
    {
        Debug.Log($"Player hp changed: {evt.CurrentHp}");
    }
}
```

说明：

- 事件类型必须实现 `IEvent`。
- `Subscribe` 返回 `IDisposable`，便于在 `OnDisable/OnDestroy` 中统一释放。
- `SubscribeOnce` 用于一次性监听。
- `EventBus.Clear<T>()` 可清空某类事件；`EventBus.Clear()` 可清空全部事件。


## Utility 小工具

Core 的 `Utility/` 目录新增了几组常用静态工具：

- `StringUtility`
  - `IsNullOrWhiteSpace(string)`：判空白字符串
  - `NullIfWhiteSpace(string)`：空白字符串转 `null`
  - `Truncate(string, int, string)`：按最大长度截断并追加后缀
- `CollectionUtility`
  - `IsNullOrEmpty(ICollection<T>)`：集合判空
  - `SafeCount(ICollection<T>)`：安全获取集合数量（null 时返回 0）
  - `TryGetValue(IReadOnlyDictionary<TKey, TValue>, TKey, out TValue)`：字典安全取值
- `MathUtility`
  - `Clamp01(float)`：限制到 `[0,1]`
  - `Approximately(float, float, float)`：带 epsilon 的近似比较
  - `Remap(float, float, float, float, float)`：区间映射
