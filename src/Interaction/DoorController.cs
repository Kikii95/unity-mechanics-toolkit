using UnityEngine;

namespace UnityMechanicsToolkit.Interaction
{
    public class DoorController : InteractableObject
    {
        public enum DoorType
        {
            Sliding,
            Rotating,
            Auto
        }

        [Header("Door Settings")]
        [SerializeField] private DoorType doorType = DoorType.Rotating;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private string requiredKeyID = "";

        [Header("Rotation Settings")]
        [SerializeField] private float rotationAngle = 90f;
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private Transform doorPivot;

        [Header("Sliding Settings")]
        [SerializeField] private Vector3 slideDirection = Vector3.right;
        [SerializeField] private float slideDistance = 2f;
        [SerializeField] private float slideSpeed = 2f;

        [Header("Auto Settings")]
        [SerializeField] private float autoCloseDelay = 3f;

        private bool isOpen = false;
        private Vector3 closedPosition;
        private Quaternion closedRotation;
        private Vector3 openPosition;
        private Quaternion openRotation;

        private float currentProgress = 0f;
        private bool isMoving = false;
        private float autoCloseTimer;

        private void Start()
        {
            if (doorPivot == null)
                doorPivot = transform;

            closedPosition = doorPivot.localPosition;
            closedRotation = doorPivot.localRotation;

            switch (doorType)
            {
                case DoorType.Sliding:
                    openPosition = closedPosition + slideDirection.normalized * slideDistance;
                    openRotation = closedRotation;
                    break;
                case DoorType.Rotating:
                    openPosition = closedPosition;
                    openRotation = closedRotation * Quaternion.Euler(0, rotationAngle, 0);
                    break;
                case DoorType.Auto:
                    openPosition = closedPosition + slideDirection.normalized * slideDistance;
                    openRotation = closedRotation;
                    break;
            }
        }

        private void Update()
        {
            if (isMoving)
            {
                float targetProgress = isOpen ? 1f : 0f;
                currentProgress = Mathf.MoveTowards(currentProgress, targetProgress,
                    (doorType == DoorType.Sliding ? slideSpeed : rotationSpeed) * Time.deltaTime);

                doorPivot.localPosition = Vector3.Lerp(closedPosition, openPosition, currentProgress);
                doorPivot.localRotation = Quaternion.Slerp(closedRotation, openRotation, currentProgress);

                if (Mathf.Approximately(currentProgress, targetProgress))
                {
                    isMoving = false;
                }
            }

            if (doorType == DoorType.Auto && isOpen)
            {
                autoCloseTimer -= Time.deltaTime;
                if (autoCloseTimer <= 0)
                {
                    CloseDoor();
                }
            }
        }

        public override void OnInteract(GameObject interactor)
        {
            if (!isInteractable) return;

            if (isLocked)
            {
                Debug.Log("Door is locked! Need key: " + requiredKeyID);
                return;
            }

            ToggleDoor();
            base.OnInteract(interactor);
        }

        public void ToggleDoor()
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        public void OpenDoor()
        {
            if (isLocked) return;

            isOpen = true;
            isMoving = true;
            autoCloseTimer = autoCloseDelay;
        }

        public void CloseDoor()
        {
            isOpen = false;
            isMoving = true;
        }

        public void Unlock(string keyID = "")
        {
            if (string.IsNullOrEmpty(requiredKeyID) || keyID == requiredKeyID)
            {
                isLocked = false;
                Debug.Log("Door unlocked!");
            }
        }

        public void Lock()
        {
            isLocked = true;
            CloseDoor();
        }

        public bool IsOpen => isOpen;
        public bool IsLocked => isLocked;
    }
}
