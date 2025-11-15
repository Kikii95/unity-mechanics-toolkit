namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Interface for objects that can die.
    /// Implemented by Health system, enemies, players, etc.
    /// </summary>
    public interface IKillable
    {
        /// <summary>
        /// Trigger death logic.
        /// </summary>
        void Die();

        /// <summary>
        /// Is this object dead?
        /// </summary>
        bool IsDead { get; }
    }
}
