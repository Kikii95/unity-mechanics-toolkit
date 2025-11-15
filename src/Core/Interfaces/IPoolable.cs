namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Interface for objects that can be pooled.
    /// Implement this on prefabs that will be spawned/despawned frequently.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Called when object is spawned from pool.
        /// Use this instead of Start() for pooled objects.
        /// </summary>
        void OnSpawn();

        /// <summary>
        /// Called when object is returned to pool.
        /// Reset state here (health, position, etc.).
        /// </summary>
        void OnDespawn();
    }
}
