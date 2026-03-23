using UnityEngine;

namespace Bill.Mutant.VR
{
    public class VRManager
    {
        private static VRManager _instance;
        public static VRManager Instance => _instance ??= new VRManager();

        private bool _vrEnabled;

        public bool IsVREnabled => _vrEnabled;

        public void EnableVR()
        {
            if (_vrEnabled) return;
            _vrEnabled = true;
            Debug.Log("[Mutant VR] VR Enabled");
        }

        public void DisableVR()
        {
            if (!_vrEnabled) return;
            _vrEnabled = false;
            Debug.Log("[Mutant VR] VR Disabled");
        }
    }
}
