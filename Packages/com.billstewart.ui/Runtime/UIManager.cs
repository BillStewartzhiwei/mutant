using System.Collections.Generic;
using UnityEngine;

namespace Bill.Mutant.UI
{
    public class UIManager
    {
        private static UIManager _instance;
        public static UIManager Instance => _instance ??= new UIManager();

        private readonly Stack<GameObject> _uiStack = new();

        public void Open(GameObject prefab)
        {
            var go = Object.Instantiate(prefab);
            _uiStack.Push(go);
        }

        public void Close()
        {
            if (_uiStack.Count == 0) return;

            var top = _uiStack.Pop();
            Object.Destroy(top);
        }
    }
}
