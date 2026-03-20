using UnityEngine;

namespace Bill.Mutant.Core
{
    public static class ModuleBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Logger.Log("Mutant Bootstrap Start");

            ModuleManager.Instance.Init();

            var go = new GameObject("[Mutant.Driver]");
            Object.DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.HideInHierarchy;

            go.AddComponent<ModuleDriver>();
        }
    }
}
