using UnityEngine;

namespace UnityMechanicsToolkit.Interaction
{
    /// <summary>
    /// Examine mechanic - 3D rotate, zoom, inspect objects.
    /// Player can examine items in detail before picking up.
    /// </summary>
    public class ExamineObject : InteractableObject
    {
        [Header("Examine Settings")]
        [SerializeField] private float examineDistance = 1f;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;

        [Header("Examine Transform")]
        [SerializeField] private Transform examinePosition; // Where object appears when examined

        private GameObject originalObject;
        private GameObject examineClone;
        private bool isExamining;
        private Vector3 originalPosition;
        private Quaternion originalRotation;

        public override void OnInteract(GameObject interactor)
        {
            if (!isInteractable) return;

            if (!isExamining)
            {
                StartExamine(interactor);
            }
        }

        private void Update()
        {
            if (isExamining)
            {
                HandleExamine();
            }
        }

        private void StartExamine(GameObject interactor)
        {
            isExamining = true;

            // Store original transform
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            // Create clone for examination
            examineClone = Instantiate(gameObject);
            examineClone.transform.position = examinePosition != null ?
                examinePosition.position : interactor.transform.position + interactor.transform.forward * examineDistance;
            examineClone.transform.rotation = Quaternion.identity;

            // Hide original
            gameObject.SetActive(false);

            Debug.Log("[ExamineObject] Examining object...");
        }

        private void HandleExamine()
        {
            if (examineClone == null) return;

            // Rotate with mouse
            if (Input.GetMouseButton(0))
            {
                float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

                examineClone.transform.Rotate(Vector3.up, -rotX, Space.World);
                examineClone.transform.Rotate(Vector3.right, rotY, Space.World);
            }

            // Exit examine
            if (Input.GetKeyDown(exitKey))
            {
                StopExamine();
            }
        }

        private void StopExamine()
        {
            isExamining = false;

            // Destroy clone
            if (examineClone != null)
            {
                Destroy(examineClone);
            }

            // Show original
            gameObject.SetActive(true);

            Debug.Log("[ExamineObject] Stopped examining.");
        }
    }
}
