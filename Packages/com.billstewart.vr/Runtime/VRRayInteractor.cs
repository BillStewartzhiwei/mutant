using UnityEngine;
using UnityEngine.EventSystems;

namespace Bill.Mutant.VR
{
    public class VRRayInteractor : MonoBehaviour
    {
        public float Distance = 10f;
        public LayerMask LayerMask;

        private void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hit, Distance, LayerMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);

                if (Input.GetMouseButtonDown(0))
                {
                    ExecuteEvents.Execute(hit.collider.gameObject,
                        new PointerEventData(EventSystem.current),
                        ExecuteEvents.pointerClickHandler);
                }
            }
        }
    }
