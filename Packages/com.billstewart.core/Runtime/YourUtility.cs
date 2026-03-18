using UnityEngine;

namespace YourCompany.YourPackage
{
    /// <summary>
    /// A utility class providing static helper methods.
    /// </summary>
    public static class YourUtility
    {
        /// <summary>
        /// Logs a formatted message.
        /// </summary>
        public static void Log(string message)
        {
            Debug.Log($"[YourPackage] {message}");
        }

        /// <summary>
        /// Example utility method: clamps a value between 0 and 1.
        /// </summary>
        public static float Clamp01(float value)
        {
            return Mathf.Clamp01(value);
        }
    }
}
