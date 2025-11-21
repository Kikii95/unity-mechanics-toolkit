using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Horror
{
    /// <summary>
    /// Flashlight system with battery drain, flicker effects, and intensity control.
    /// Core horror mechanic for darkness gameplay.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class FlashlightSystem : MonoBehaviour
    {
        [Header("Light Settings")]
        [SerializeField] private Light flashlight;
        [SerializeField] private KeyCode toggleKey = KeyCode.F;
        [SerializeField] private float maxIntensity = 2f;
        [SerializeField] private float maxRange = 15f;

        [Header("Battery")]
        [SerializeField] private float maxBattery = 100f;
        [SerializeField] private float currentBattery;
        [SerializeField] private float drainRate = 5f; // Per second when ON
        [SerializeField] private float lowBatteryThreshold = 20f;

        [Header("Flicker")]
        [SerializeField] private bool flickerWhenLow = true;
        [SerializeField] private float flickerIntensity = 0.5f;
        [SerializeField] private float flickerSpeed = 10f;

        [Header("Events")]
        public UnityEvent<float> onBatteryChanged;
        public UnityEvent onBatteryDepleted;
        public UnityEvent onFlashlightToggled;

        private bool isOn;
        private float flickerTimer;

        public bool IsOn => isOn;
        public float CurrentBattery => currentBattery;
        public float BatteryPercentage => currentBattery / maxBattery;
        public bool IsLowBattery => currentBattery < lowBatteryThreshold;

        private void Awake()
        {
            if (flashlight == null)
            {
                flashlight = GetComponent<Light>();
            }

            currentBattery = maxBattery;
            flashlight.enabled = false;
            isOn = false;
        }

        private void Update()
        {
            HandleInput();
            HandleBatteryDrain();
            HandleFlicker();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            if (isOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }

            onFlashlightToggled?.Invoke();
        }

        public void TurnOn()
        {
            if (currentBattery <= 0)
            {
                return; // No battery
            }

            isOn = true;
            flashlight.enabled = true;
        }

        public void TurnOff()
        {
            isOn = false;
            flashlight.enabled = false;
        }

        private void HandleBatteryDrain()
        {
            if (!isOn) return;

            currentBattery -= drainRate * Time.deltaTime;

            if (currentBattery <= 0)
            {
                currentBattery = 0;
                TurnOff();
                onBatteryDepleted?.Invoke();
            }

            onBatteryChanged?.Invoke(currentBattery);

            // Reduce intensity as battery drains
            float batteryFactor = currentBattery / maxBattery;
            flashlight.range = maxRange * Mathf.Max(batteryFactor, 0.3f);
        }

        private void HandleFlicker()
        {
            if (!isOn || !flickerWhenLow) return;

            if (IsLowBattery)
            {
                flickerTimer += Time.deltaTime * flickerSpeed;

                // Random flicker
                float flicker = Mathf.PerlinNoise(flickerTimer, 0f);
                float intensityVariation = Mathf.Lerp(maxIntensity * flickerIntensity, maxIntensity, flicker);

                flashlight.intensity = intensityVariation;

                // Occasionally turn off briefly
                if (flicker < 0.1f)
                {
                    flashlight.enabled = false;
                }
                else
                {
                    flashlight.enabled = true;
                }
            }
            else
            {
                flashlight.intensity = maxIntensity;
            }
        }

        public void AddBattery(float amount)
        {
            amount = Mathf.Abs(amount);
            currentBattery = Mathf.Min(maxBattery, currentBattery + amount);
            onBatteryChanged?.Invoke(currentBattery);
        }

        public void SetBattery(float amount)
        {
            currentBattery = Mathf.Clamp(amount, 0, maxBattery);
            onBatteryChanged?.Invoke(currentBattery);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (flashlight == null) return;

            Gizmos.color = isOn ? (IsLowBattery ? Color.yellow : Color.green) : Color.red;
            Gizmos.DrawWireSphere(transform.position, flashlight.range);
            Gizmos.DrawRay(transform.position, transform.forward * flashlight.range);
        }
#endif
    }
}
