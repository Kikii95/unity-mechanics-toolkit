namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Interface for objects that can take damage.
    /// Implemented by Health system, enemies, destructible objects, etc.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Apply damage to this object.
        /// </summary>
        /// <param name="amount">Damage amount</param>
        void TakeDamage(float amount);

        /// <summary>
        /// Current health value.
        /// </summary>
        float CurrentHealth { get; }
    }
}
