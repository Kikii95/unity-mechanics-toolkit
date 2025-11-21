using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.AI
{
    /// <summary>
    /// State machine for AI behavior (Idle, Patrol, Chase, Attack, Search).
    /// Core system for enemy AI logic.
    /// </summary>
    public class AIStateMachine : MonoBehaviour
    {
        public enum AIState
        {
            Idle,
            Patrol,
            Chase,
            Attack,
            Search,
            Dead
        }

        [Header("Current State")]
        [SerializeField] private AIState currentState = AIState.Idle;

        [Header("Events")]
        public UnityEvent<AIState> onStateChanged;
        public UnityEvent onStateIdle;
        public UnityEvent onStatePatrol;
        public UnityEvent onStateChase;
        public UnityEvent onStateAttack;
        public UnityEvent onStateSearch;
        public UnityEvent onStateDead;

        public AIState CurrentState => currentState;

        private void Start()
        {
            ChangeState(currentState);
        }

        /// <summary>
        /// Change AI state with events.
        /// </summary>
        public void ChangeState(AIState newState)
        {
            if (currentState == newState) return;

            // Exit previous state
            ExitState(currentState);

            // Update state
            currentState = newState;

            // Enter new state
            EnterState(newState);

            // Trigger events
            onStateChanged?.Invoke(newState);
        }

        private void EnterState(AIState state)
        {
            switch (state)
            {
                case AIState.Idle:
                    onStateIdle?.Invoke();
                    break;
                case AIState.Patrol:
                    onStatePatrol?.Invoke();
                    break;
                case AIState.Chase:
                    onStateChase?.Invoke();
                    break;
                case AIState.Attack:
                    onStateAttack?.Invoke();
                    break;
                case AIState.Search:
                    onStateSearch?.Invoke();
                    break;
                case AIState.Dead:
                    onStateDead?.Invoke();
                    break;
            }

            Debug.Log($"[AI] Entered state: {state}");
        }

        private void ExitState(AIState state)
        {
            Debug.Log($"[AI] Exited state: {state}");
        }

        // Public state transition methods
        public void StartIdle() => ChangeState(AIState.Idle);
        public void StartPatrol() => ChangeState(AIState.Patrol);
        public void StartChase() => ChangeState(AIState.Chase);
        public void StartAttack() => ChangeState(AIState.Attack);
        public void StartSearch() => ChangeState(AIState.Search);
        public void Die() => ChangeState(AIState.Dead);

        public bool IsInState(AIState state) => currentState == state;
        public bool IsIdle() => currentState == AIState.Idle;
        public bool IsPatrolling() => currentState == AIState.Patrol;
        public bool IsChasing() => currentState == AIState.Chase;
        public bool IsAttacking() => currentState == AIState.Attack;
        public bool IsSearching() => currentState == AIState.Search;
        public bool IsDead() => currentState == AIState.Dead;
    }
}
