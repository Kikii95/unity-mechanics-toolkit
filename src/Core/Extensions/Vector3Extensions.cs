using UnityEngine;

namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Extension methods for Vector3.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Flatten vector (set Y to 0). Useful for horizontal calculations.
        /// </summary>
        public static Vector3 Flat(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }

        /// <summary>
        /// Get random point within radius around center.
        /// </summary>
        public static Vector3 RandomInRadius(this Vector3 center, float radius)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            return center + new Vector3(randomCircle.x, 0, randomCircle.y);
        }

        /// <summary>
        /// Get direction to target (normalized).
        /// </summary>
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        /// <summary>
        /// Get distance to target.
        /// </summary>
        public static float DistanceTo(this Vector3 from, Vector3 to)
        {
            return Vector3.Distance(from, to);
        }

        /// <summary>
        /// Clamp magnitude of vector.
        /// </summary>
        public static Vector3 ClampMagnitude(this Vector3 v, float min, float max)
        {
            float magnitude = v.magnitude;
            if (magnitude < min)
            {
                return v.normalized * min;
            }
            else if (magnitude > max)
            {
                return v.normalized * max;
            }
            return v;
        }
    }
}
