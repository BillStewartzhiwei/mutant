using UnityEngine;
using Mutant.Core.Config;
using Mutant.Core.Events;
using Mutant.Core.Modules;

namespace Mutant.Core.Bootstrap
{
    public sealed class CoreBootstrap : MonoBehaviour
    {
        [SerializeField] private CoreSettings settings;

        private static CoreBootstrap _instance;
        private bool _bootstrapped;

        private void Awake()
        {
            if (settings == null)
            {
                Debug.LogError("[CoreBootstrap] CoreSettings is missing.");
                return;
            }

            if (settings.preventDuplicateBootstrap)
            {
                if (_instance != null && _instance != this)
                {
                    Destroy(gameObject);
                    return;
                }

                _instance = this;
            }

            if (settings.dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            ModuleManager.Instance.Configure(settings.logLifecycle);
            EventBus.Configure(settings.logEventBus);

            if (settings.enableAutoRegister)
                ModuleManager.Instance.AutoRegisterAttributedModules();

            ModuleManager.Instance.InitAll();
            _bootstrapped = true;
        }

        private void Update()
        {
            if (_bootstrapped)
                ModuleManager.Instance.UpdateAll();
        }

        private void LateUpdate()
        {
            if (_bootstrapped)
                ModuleManager.Instance.LateUpdateAll();
        }

        private void FixedUpdate()
        {
            if (_bootstrapped)
                ModuleManager.Instance.FixedUpdateAll();
        }

        private void OnDestroy()
        {
            if (_bootstrapped)
            {
                ModuleManager.Instance.DisposeAll();
                EventBus.ClearAll();
                StickyEventBus.ClearAll();
                _bootstrapped = false;
            }

            if (_instance == this)
                _instance = null;
        }
    }
}