using UnityEngine;

namespace Bill.Mutant.Core
{
    public static class ModuleBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
	        var logConfig = Resources.Load<LogConfig>("Mutant/LogConfig");
	        if (logConfig == null)
	        {
		        logConfig = ScriptableObject.CreateInstance<LogConfig>();
	        }

	        Logger.Init(logConfig);
	        Logger.Info(LogCategory.Core, "Mutant Bootstrap Start", nameof(ModuleBootstrap));

	        ModuleManager.Instance.Init();

	        var go = new GameObject("[Mutant.Driver]");
	        Object.DontDestroyOnLoad(go);
	        go.hideFlags = HideFlags.HideInHierarchy;
	        go.AddComponent<ModuleDriver>();
        }
    }
}
