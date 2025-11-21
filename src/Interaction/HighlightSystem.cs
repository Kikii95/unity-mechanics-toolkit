using UnityEngine;

namespace UnityMechanicsToolkit.Interaction
{
    /// <summary>
    /// Outline/highlight system for interactable objects.
    /// Simple emission-based highlighting (can be upgraded with outline shaders later).
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class HighlightSystem : MonoBehaviour
    {
        [Header("Highlight Settings")]
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private float highlightIntensity = 2f;

        private Renderer objectRenderer;
        private Material originalMaterial;
        private Color originalEmission;
        private bool isHighlighted;

        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalMaterial = objectRenderer.material;
                originalEmission = originalMaterial.GetColor("_EmissionColor");
            }
        }

        public void EnableHighlight()
        {
            if (isHighlighted || objectRenderer == null) return;

            objectRenderer.material.EnableKeyword("_EMISSION");
            objectRenderer.material.SetColor("_EmissionColor", highlightColor * highlightIntensity);
            isHighlighted = true;
        }

        public void DisableHighlight()
        {
            if (!isHighlighted || objectRenderer == null) return;

            objectRenderer.material.SetColor("_EmissionColor", originalEmission);
            isHighlighted = false;
        }

        private void OnDestroy()
        {
            if (objectRenderer != null)
            {
                Destroy(objectRenderer.material);
            }
        }
    }
}
