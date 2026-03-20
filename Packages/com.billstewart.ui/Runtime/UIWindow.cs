using UnityEngine;

namespace Bill.Mutant.UI
{
    public class UIWindow : UIView
    {
        public int Layer { get; set; }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }
    }
}
