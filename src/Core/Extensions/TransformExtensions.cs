using UnityEngine;

namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Extension methods for Transform component.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Reset transform to default values (position 0, rotation identity, scale 1).
        /// </summary>
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Find child by name recursively (searches entire hierarchy).
        /// </summary>
        public static Transform FindDeep(this Transform parent, string name)
        {
            Transform result = parent.Find(name);
            if (result != null) return result;

            foreach (Transform child in parent)
            {
                result = child.FindDeep(name);
                if (result != null) return result;
            }

            return null;
        }

        /// <summary>
        /// Destroy all children.
        /// </summary>
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Destroy all children immediately (editor-safe).
        /// </summary>
        public static void DestroyChildrenImmediate(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}
