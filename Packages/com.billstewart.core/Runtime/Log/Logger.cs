using UnityEngine;

namespace Bill.Mutant.Core
{
    public static class Logger
    {
        public static bool EnableLog = true;

        public static void Log(string msg)
        {
            if (EnableLog)
            {
                Debug.Log("[Mutant] " + msg);
            }
        }

        public static void Warning(string msg)
        {
            Debug.LogWarning("[Mutant] " + msg);
        }

        public static void Error(string msg)
        {
            Debug.LogError("[Mutant] " + msg);
        }
    }
}
