using UnityEngine;
using UnityEngine.Events;
using UnityMechanicsToolkit.Core;

namespace UnityMechanicsToolkit.AI
{
    /// <summary>
    /// Vision cone detection system for AI.
    /// Detects targets within FOV angle and range, with line of sight checks.
    /// </summary>
    public class VisionCone : MonoBehaviour
    {
        [Header("Vision Settings")]
        [SerializeField] private float visionRange = 10f;
        [SerializeField] private float visionAngle = 90f; // Total angle (45° each side)
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask obstructionLayer;

        [Header("Detection")]
        [SerializeField] private float detectionInterval = 0.2f; // Check every 0.2s (performance)
        [SerializeField] private string targetTag = "Player";

        [Header("Events")]
        public UnityEvent<GameObject> onTargetDetected;
        public UnityEvent onTargetLost;

        private GameObject currentTarget;
        private float detectionTimer;

        public GameObject CurrentTarget => currentTarget;
        public bool HasTarget => currentTarget != null;

        private void Update()
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= detectionInterval)
            {
                detectionTimer = 0f;
                CheckForTarget();
            }
        }

        private void CheckForTarget()
        {
            // Find all potential targets in range
            Collider[] targetsInRange = Physics.OverlapSphere(transform.position, visionRange, targetLayer);

            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider target in targetsInRange)
            {
                // Check if correct tag
                if (!string.IsNullOrEmpty(targetTag) && !target.CompareTag(targetTag))
                {
                    continue;
                }

                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                // Check if within FOV angle
                if (angleToTarget <= visionAngle / 2f)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // Check line of sight (not obstructed)
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                    {
                        // Found valid target
                        if (distanceToTarget < closestDistance)
                        {
                            closestTarget = target.gameObject;
                            closestDistance = distanceToTarget;
                        }
                    }
                }
            }

            // Update current target
            if (closestTarget != null)
            {
                if (currentTarget != closestTarget)
                {
                    currentTarget = closestTarget;
                    onTargetDetected?.Invoke(currentTarget);
                    GameEvents.AIDetectedPlayer(gameObject, currentTarget.transform.position);
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget = null;
                    onTargetLost?.Invoke();
                    GameEvents.AILostPlayer(gameObject);
                }
            }
        }

        public bool CanSeeTarget(GameObject target)
        {
            if (target == null) return false;

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check range, angle, and line of sight
            if (distanceToTarget <= visionRange &&
                angleToTarget <= visionAngle / 2f &&
                !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
            {
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Draw vision range
            Gizmos.color = HasTarget ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRange);

            // Draw vision cone
            Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward * visionRange;
            Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward * visionRange;

            Gizmos.color = HasTarget ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, leftBoundary);
            Gizmos.DrawRay(transform.position, rightBoundary);
            Gizmos.DrawRay(transform.position, transform.forward * visionRange);

            // Draw line to current target
            if (HasTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, currentTarget.transform.position);
            }
        }
#endif
    }
}
