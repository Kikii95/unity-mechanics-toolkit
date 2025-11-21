using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Stamina/Energy system for sprint, dodge, melee attacks, etc.
    /// Modular - can be used standalone or integrated with movement/combat.
    /// </summary>
    public class StaminaSystem : MonoBehaviour
    {
        [Header("Stamina Settings")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float currentStamina;

        [Header("Regeneration")]
        [SerializeField] private bool canRegenerate = true;
        [SerializeField] private float regenRate = 10f; // Per second
        [SerializeField] private float regenDelay = 2f; // Delay after consumption

        [Header("Drain Settings")]
        [SerializeField] private float minStaminaThreshold = 0f; // Can't use abilities below this

        [Header("Events")]
        public UnityEvent<float> onStaminaChanged;
        public UnityEvent onStaminaDepleted;
        public UnityEvent onStaminaRestored;

        // Public properties
        public float MaxStamina => maxStamina;
        public float CurrentStamina => currentStamina;
        public float StaminaPercentage => currentStamina / maxStamina;
        public bool IsDepleted => currentStamina <= minStaminaThreshold;
        public bool IsFull => Mathf.Approximately(currentStamina, maxStamina);

        private float timeSinceLastConsumption;
        private bool wasDepleted;

        private void Awake()
        {
            currentStamina = maxStamina;
        }

        private void Update()
        {
            HandleRegeneration();
            CheckDepletedState();
        }

        /// <summary>
        /// Try to consume stamina. Returns true if successful.
        /// </summary>
        public bool TryConsume(float amount)
        {
            if (currentStamina < amount)
            {
                return false; // Not enough stamina
            }

            Consume(amount);
            return true;
        }

        /// <summary>
        /// Consume stamina (no check). Use TryConsume for safe consumption.
        /// </summary>
        public void Consume(float amount)
        {
            amount = Mathf.Abs(amount);
            currentStamina = Mathf.Max(0, currentStamina - amount);
            timeSinceLastConsumption = 0f;

            onStaminaChanged?.Invoke(currentStamina);
        }

        /// <summary>
        /// Restore stamina.
        /// </summary>
        public void Restore(float amount)
        {
            amount = Mathf.Abs(amount);
            float previousStamina = currentStamina;
            currentStamina = Mathf.Min(maxStamina, currentStamina + amount);

            if (currentStamina > previousStamina)
            {
                onStaminaChanged?.Invoke(currentStamina);
            }
        }

        /// <summary>
        /// Check if enough stamina is available.
        /// </summary>
        public bool HasStamina(float amount)
        {
            return currentStamina >= amount;
        }

        /// <summary>
        /// Fully restore stamina.
        /// </summary>
        public void RestoreFull()
        {
            currentStamina = maxStamina;
            onStaminaChanged?.Invoke(currentStamina);
        }

        /// <summary>
        /// Set max stamina (optional: restore to new max).
        /// </summary>
        public void SetMaxStamina(float newMax, bool restoreToMax = false)
        {
            maxStamina = Mathf.Max(1f, newMax);

            if (restoreToMax)
            {
                currentStamina = maxStamina;
            }
            else
            {
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }

            onStaminaChanged?.Invoke(currentStamina);
        }

        private void HandleRegeneration()
        {
            if (!canRegenerate || IsFull) return;

            timeSinceLastConsumption += Time.deltaTime;

            if (timeSinceLastConsumption >= regenDelay)
            {
                Restore(regenRate * Time.deltaTime);
            }
        }

        private void CheckDepletedState()
        {
            bool isDepleted = IsDepleted;

            if (isDepleted && !wasDepleted)
            {
                onStaminaDepleted?.Invoke();
                wasDepleted = true;
            }
            else if (!isDepleted && wasDepleted)
            {
                onStaminaRestored?.Invoke();
                wasDepleted = false;
            }
        }
    }
}
