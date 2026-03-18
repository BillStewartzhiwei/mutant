using UnityEditor;

namespace YourCompany.YourPackage.Editor
{
    /// <summary>
    /// Adds menu items for the package under Tools menu.
    /// </summary>
    public static class YourPackageMenu
    {
        [MenuItem("Tools/YourPackage/Say Hello")]
        private static void SayHello()
        {
            YourUtility.Log("Hello from the menu!");
        }

        [MenuItem("Tools/YourPackage/Open Documentation")]
        private static void OpenDocumentation()
        {
            UnityEngine.Application.OpenURL("https://your-website.com/docs");
        }
    }
}
