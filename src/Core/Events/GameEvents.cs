using System;
using UnityEngine;

namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Centralized event system for cross-module communication.
    /// Modules can publish/subscribe to events without direct dependencies.
    /// </summary>
    public static class GameEvents
    {
        // ===== PLAYER EVENTS =====

        /// <summary>Triggered when player health changes. Param: new health value</summary>
        public static event Action<float> OnPlayerHealthChanged;

        /// <summary>Triggered when player dies.</summary>
        public static event Action OnPlayerDeath;

        /// <summary>Triggered when player makes a footstep. Params: position, volume (0-1)</summary>
        public static event Action<Vector3, float> OnPlayerFootstep;

        /// <summary>Triggered when player sprints. Param: is sprinting</summary>
        public static event Action<bool> OnPlayerSprint;

        // ===== COMBAT EVENTS =====

        /// <summary>Triggered when damage is dealt. Params: target, amount</summary>
        public static event Action<GameObject, float> OnDamageDealt;

        /// <summary>Triggered when enemy is killed. Param: enemy GameObject</summary>
        public static event Action<GameObject> OnEnemyKilled;

        /// <summary>Triggered when weapon is fired. Params: weapon, position</summary>
        public static event Action<GameObject, Vector3> OnWeaponFired;

        // ===== INTERACTION EVENTS =====

        /// <summary>Triggered when player interacts with object. Param: interactable</summary>
        public static event Action<IInteractable> OnInteraction;

        /// <summary>Triggered when door opens/closes. Params: door, is open</summary>
        public static event Action<GameObject, bool> OnDoorStateChanged;

        // ===== INVENTORY EVENTS =====

        /// <summary>Triggered when item is picked up. Params: item, quantity</summary>
        public static event Action<object, int> OnItemPickedUp;

        /// <summary>Triggered when item is used. Param: item</summary>
        public static event Action<object> OnItemUsed;

        // ===== AI EVENTS =====

        /// <summary>Triggered when AI detects player. Params: AI GameObject, player position</summary>
        public static event Action<GameObject, Vector3> OnAIDetectedPlayer;

        /// <summary>Triggered when AI loses player. Param: AI GameObject</summary>
        public static event Action<GameObject> OnAILostPlayer;

        // ===== TRIGGER METHODS =====
        // Call these from modules to emit events

        public static void PlayerHealthChanged(float health) => OnPlayerHealthChanged?.Invoke(health);
        public static void PlayerDied() => OnPlayerDeath?.Invoke();
        public static void PlayerFootstep(Vector3 position, float volume) => OnPlayerFootstep?.Invoke(position, volume);
        public static void PlayerSprint(bool isSprinting) => OnPlayerSprint?.Invoke(isSprinting);

        public static void DamageDealt(GameObject target, float amount) => OnDamageDealt?.Invoke(target, amount);
        public static void EnemyKilled(GameObject enemy) => OnEnemyKilled?.Invoke(enemy);
        public static void WeaponFired(GameObject weapon, Vector3 position) => OnWeaponFired?.Invoke(weapon, position);

        public static void Interaction(IInteractable interactable) => OnInteraction?.Invoke(interactable);
        public static void DoorStateChanged(GameObject door, bool isOpen) => OnDoorStateChanged?.Invoke(door, isOpen);

        public static void ItemPickedUp(object item, int quantity) => OnItemPickedUp?.Invoke(item, quantity);
        public static void ItemUsed(object item) => OnItemUsed?.Invoke(item);

        public static void AIDetectedPlayer(GameObject ai, Vector3 playerPosition) => OnAIDetectedPlayer?.Invoke(ai, playerPosition);
        public static void AILostPlayer(GameObject ai) => OnAILostPlayer?.Invoke(ai);
    }
}
