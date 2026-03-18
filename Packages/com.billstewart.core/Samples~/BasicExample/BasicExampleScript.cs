using UnityEngine;
using YourCompany.YourPackage;

/// <summary>
/// Basic example: attach this to a GameObject to see the package in action.
/// </summary>
public class BasicExampleScript : MonoBehaviour
{
    private void Start()
    {
        YourUtility.Log("Basic Example is running!");

        // Access the component if attached
        var component = GetComponent<YourComponent>();
        if (component != null)
        {
            component.DoSomething();
        }
    }
}
