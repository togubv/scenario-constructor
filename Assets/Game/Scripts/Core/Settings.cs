using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    public class Settings
    {
        private static Dictionary<Type, ScriptableObject> _pools;

        public static T Get<T>() where T : ScriptableObject
        {
            if (_pools != null &&
                _pools.TryGetValue(typeof(T), out var config))
            {
                return (T)config;
            }

            return default;
        }

        public void InitializeSettings()
        {
            _pools = new Dictionary<Type, ScriptableObject>();

            var allAssets = Resources.LoadAll<ScriptableObject>("Data/Pools");

            foreach (var obj in allAssets)
            {
                var type = obj.GetType();

                if (_pools.ContainsKey(type) == false)
                {
                    _pools.Add(type, obj);
                }

                if (obj is ILoadablePool loadablePool)
                {
                    loadablePool.Load();
                }
            }
        }
    }
}
