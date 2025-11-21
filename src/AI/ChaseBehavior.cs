using UnityEngine;
using UnityEngine.AI;

namespace UnityMechanicsToolkit.AI
{
    /// <summary>
    /// Chase behavior for AI - follows target using NavMeshAgent.
    /// Requires NavMeshAgent, AIStateMachine, and VisionCone.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AIStateMachine))]
    public class ChaseBehavior : MonoBehaviour
    {
        [Header("Chase Settings")]
        [SerializeField] private float chaseSpeed = 5f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float giveUpDistance = 20f; // Distance before giving up chase
        [SerializeField] private float lostTargetSearchTime = 5f;

        private NavMeshAgent agent;
        private AIStateMachine stateMachine;
        private VisionCone visionCone;
        private GameObject target;
        private Vector3 lastKnownPosition;
        private float lostTargetTimer;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            stateMachine = GetComponent<AIStateMachine>();
            visionCone = GetComponent<VisionCone>();
        }

        private void OnEnable()
        {
            if (stateMachine != null)
            {
                stateMachine.onStateChase.AddListener(OnChaseStart);
            }

            if (visionCone != null)
            {
                visionCone.onTargetDetected.AddListener(OnTargetDetected);
                visionCone.onTargetLost.AddListener(OnTargetLost);
            }
        }

        private void OnDisable()
        {
            if (stateMachine != null)
            {
                stateMachine.onStateChase.RemoveListener(OnChaseStart);
            }

            if (visionCone != null)
            {
                visionCone.onTargetDetected.RemoveListener(OnTargetDetected);
                visionCone.onTargetLost.RemoveListener(OnTargetLost);
            }
        }

        private void Update()
        {
            if (stateMachine != null && !stateMachine.IsChasing()) return;

            Chase();
        }

        private void OnChaseStart()
        {
            agent.speed = chaseSpeed;
            lostTargetTimer = 0f;
        }

        private void OnTargetDetected(GameObject detectedTarget)
        {
            target = detectedTarget;
            lastKnownPosition = target.transform.position;

            if (stateMachine != null && !stateMachine.IsChasing())
            {
                stateMachine.StartChase();
            }
        }

        private void OnTargetLost()
        {
            // Start search mode (go to last known position)
            if (stateMachine != null && stateMachine.IsChasing())
            {
                stateMachine.StartSearch();
            }
        }

        private void Chase()
        {
            if (target == null)
            {
                // Lost target - search
                HandleLostTarget();
                return;
            }

            // Update last known position
            lastKnownPosition = target.transform.position;

            // Move toward target
            agent.SetDestination(target.transform.position);

            // Check if in attack range
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget <= attackRange)
            {
                // Enter attack state
                if (stateMachine != null)
                {
                    stateMachine.StartAttack();
                }
            }
            else if (distanceToTarget > giveUpDistance)
            {
                // Too far, give up chase
                target = null;
                if (stateMachine != null)
                {
                    stateMachine.StartPatrol();
                }
            }
        }

        private void HandleLostTarget()
        {
            // Go to last known position
            agent.SetDestination(lastKnownPosition);

            lostTargetTimer += Time.deltaTime;

            // If reached last known position and still no target, give up
            if (!agent.pathPending && agent.remainingDistance <= 1f)
            {
                if (lostTargetTimer >= lostTargetSearchTime)
                {
                    if (stateMachine != null)
                    {
                        stateMachine.StartPatrol();
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, giveUpDistance);

            if (target != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }
#endif
    }
}
