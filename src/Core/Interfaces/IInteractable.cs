using UnityEngine;

namespace UnityMechanicsToolkit.Core
{
    /// <summary>
    /// Interface for interactable objects.
    /// Implemented by doors, items, switches, NPCs, etc.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Called when player interacts with this object.
        /// </summary>
        /// <param name="interactor">GameObject that initiated interaction (usually Player)</param>
        void Interact(GameObject interactor);

        /// <summary>
        /// Get text to display for interaction prompt.
        /// </summary>
        /// <returns>Prompt text (e.g., "Press E to open door")</returns>
        string GetInteractionPrompt();

        /// <summary>
        /// Can this object be interacted with right now?
        /// </summary>
        bool CanInteract { get; }
    }
}
