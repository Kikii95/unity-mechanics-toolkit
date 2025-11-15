using UnityEngine;

namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Extension methods for GameObject.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Get or add component. Returns existing component if found, adds new if not.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                component = obj.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Check if GameObject has component.
        /// </summary>
        public static bool HasComponent<T>(this GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }

        /// <summary>
        /// Set layer recursively for GameObject and all children.
        /// </summary>
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        /// <summary>
        /// Destroy component if it exists.
        /// </summary>
        public static void DestroyComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            if (component != null)
            {
                Object.Destroy(component);
            }
        }
    }
}
