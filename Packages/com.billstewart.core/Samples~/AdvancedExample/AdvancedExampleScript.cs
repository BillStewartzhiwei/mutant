using UnityEngine;
using YourCompany.YourPackage;

/// <summary>
/// Advanced example: demonstrates more complex usage patterns.
/// </summary>
public class AdvancedExampleScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private YourComponent targetComponent;

    private void Start()
    {
        YourUtility.Log("Advanced Example is running!");
    }

    private void Update()
    {
        // Example: rotate the object
        transform.Rotate(Vector3.up, speed * Time.deltaTime);

        // Example: use utility
        float normalized = YourUtility.Clamp01(Time.time % 2f);
        // You could use this value to drive some visual effect
    }

    /// <summary>
    /// Called by a UI button or event to trigger the component.
    /// </summary>
    public void TriggerAction()
    {
        if (targetComponent != null)
        {
            targetComponent.DoSomething();
        }
        else
        {
            YourUtility.Log("No target component assigned.");
        }
    }
}
