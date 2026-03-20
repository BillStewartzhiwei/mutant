using UnityEngine;

namespace Bill.Mutant.UI
{
    public abstract class UIView : MonoBehaviour
    {
        public virtual void OnOpen() { }
        public virtual void OnClose() { }

        public void Open()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        public void Close()
        {
            OnClose();
            gameObject.SetActive(false);
        }
    }
}
