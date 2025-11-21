using UnityEngine;
using UnityEngine.AI;

namespace UnityMechanicsToolkit.AI
{
    /// <summary>
    /// Patrol behavior for AI using waypoints.
    /// Requires NavMeshAgent and AIStateMachine.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AIStateMachine))]
    public class PatrolBehavior : MonoBehaviour
    {
        [Header("Patrol Settings")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float waypointReachedDistance = 0.5f;
        [SerializeField] private float waitTimeAtWaypoint = 2f;
        [SerializeField] private bool loopWaypoints = true;
        [SerializeField] private bool randomOrder = false;

        [Header("Movement")]
        [SerializeField] private float patrolSpeed = 2f;

        private NavMeshAgent agent;
        private AIStateMachine stateMachine;
        private int currentWaypointIndex;
        private float waitTimer;
        private bool isWaiting;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            stateMachine = GetComponent<AIStateMachine>();
        }

        private void OnEnable()
        {
            if (stateMachine != null)
            {
                stateMachine.onStatePatrol.AddListener(OnPatrolStart);
            }
        }

        private void OnDisable()
        {
            if (stateMachine != null)
            {
                stateMachine.onStatePatrol.RemoveListener(OnPatrolStart);
            }
        }

        private void Update()
        {
            if (stateMachine != null && !stateMachine.IsPatrolling()) return;
            if (waypoints == null || waypoints.Length == 0) return;

            Patrol();
        }

        private void OnPatrolStart()
        {
            agent.speed = patrolSpeed;
            isWaiting = false;
            waitTimer = 0f;

            if (waypoints.Length > 0)
            {
                GoToNextWaypoint();
            }
        }

        private void Patrol()
        {
            if (isWaiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeAtWaypoint)
                {
                    isWaiting = false;
                    GoToNextWaypoint();
                }
            }
            else
            {
                // Check if reached waypoint
                if (!agent.pathPending && agent.remainingDistance <= waypointReachedDistance)
                {
                    isWaiting = true;
                    waitTimer = 0f;
                }
            }
        }

        private void GoToNextWaypoint()
        {
            if (waypoints.Length == 0) return;

            if (randomOrder)
            {
                currentWaypointIndex = Random.Range(0, waypoints.Length);
            }
            else
            {
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Length)
                {
                    if (loopWaypoints)
                    {
                        currentWaypointIndex = 0;
                    }
                    else
                    {
                        currentWaypointIndex = waypoints.Length - 1;
                        return; // Stop at last waypoint
                    }
                }
            }

            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Gizmos.color = Color.blue;

            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;

                // Draw waypoint sphere
                Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);

                // Draw path between waypoints
                if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else if (loopWaypoints && i == waypoints.Length - 1 && waypoints[0] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
#endif
    }
}
