using UnityEngine;

namespace Bill.Mutant.Core
{
    internal sealed class ModuleDriver : MonoBehaviour
    {
        private void Update()
        {
            ModuleManager.Instance.Update(Time.deltaTime);
        }

        private void OnApplicationQuit()
        {
            ModuleManager.Instance.Dispose();
        }
    }
}
