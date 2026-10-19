using UnityEngine;

namespace SC
{
    public class ObjectFactory
    {
        public static T Create<T>(string prefabPath, Transform parentTrans = null)
        {
            var go = (GameObject)Object.Instantiate(Resources.Load(prefabPath));

            if (go == null)
            {
                Debug.LogError($"Failed to load prefab at path {prefabPath}");
                return default;
            }

            if (parentTrans != null)
            {
                go.transform.SetParent(parentTrans, false);
            }

            var result = go.GetComponent<T>() ?? go.GetComponentInChildren<T>();

            return result;
        }
    }
}
