using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bill.Mutant.UI
{
    public class UIManagerV2
    {
        private static UIManagerV2 _instance;
        public static UIManagerV2 Instance => _instance ??= new UIManagerV2();

        private readonly Dictionary<Type, UIView> _uiCache = new();
        private readonly Dictionary<UILayer, Transform> _layerRoots = new();

        public void RegisterLayerRoot(UILayer layer, Transform root)
        {
            _layerRoots[layer] = root;
        }

        public T Open<T>(UILayer layer = UILayer.Normal) where T : UIView
        {
            var type = typeof(T);
            if (_uiCache.TryGetValue(type, out var existing))
            {
                existing.Open();
                return (T)existing;
            }

            var prefab = Resources.Load<T>(type.Name);
            if (prefab == null)
            {
                Debug.LogError($"Prefab {type.Name} not found in Resources.");
                return null;
            }

            var root = _layerRoots.ContainsKey(layer) ? _layerRoots[layer] : null;
            var go = GameObject.Instantiate(prefab.gameObject, root);
            var view = go.GetComponent<T>();
            view.Open();

            _uiCache[type] = view;
            return view;
        }

        public void Close<T>() where T : UIView
        {
            var type = typeof(T);
            if (_uiCache.TryGetValue(type, out var view))
            {
                view.Close();
            }
        }

        public void CloseAll()
        {
            foreach (var view in _uiCache.Values)
                view.Close();
        }
    }
}
