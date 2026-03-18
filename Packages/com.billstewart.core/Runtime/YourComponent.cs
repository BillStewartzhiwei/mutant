using UnityEngine;

namespace YourCompany.YourPackage
{
    /// <summary>
    /// A sample MonoBehaviour demonstrating the package's core functionality.
    /// </summary>
    public class YourComponent : MonoBehaviour
    {
        [SerializeField] private string message = "Hello from YourPackage!";

        private void Start()
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Example public method.
        /// </summary>
        public void DoSomething()
        {
            Debug.Log($"[YourPackage] DoSomething called: {message}");
        }
    }
}
