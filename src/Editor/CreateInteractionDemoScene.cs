using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityMechanicsToolkit.Interaction;

namespace UnityMechanicsToolkit.Editor
{
    /// <summary>
    /// Editor tool to automatically create an interaction systems demo scene.
    /// Menu: Tools > Unity Mechanics Toolkit > Create Interaction Demo Scene
    /// </summary>
    public static class CreateInteractionDemoScene
    {
        [MenuItem("Tools/Unity Mechanics Toolkit/Create Demo Scenes/Interaction Systems")]
        public static void Create()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // Create ground
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(10, 1, 10);
            ground.isStatic = true;

            // Create simple player with RaycastInteractor
            GameObject player = CreatePlayerWithInteractor();

            // Create Door
            CreateDoor(new Vector3(0, 1, 5));

            // Create Examine Object (Key)
            CreateExamineObject(new Vector3(3, 1, 0));

            // Create Hiding Spot (Closet)
            CreateHidingSpot(new Vector3(-5, 0, 0));

            // Create light
            GameObject dirLight = new GameObject("Directional Light");
            dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light dirLightComponent = dirLight.AddComponent<Light>();
            dirLightComponent.type = LightType.Directional;

            Debug.Log("[Unity Mechanics Toolkit] Interaction Demo Scene created successfully!");
            Debug.Log("Controls: Look at objects, Press E to interact");
            Debug.Log("Features: Door (open/close), Key (examine), Closet (hide)");

            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static GameObject CreatePlayerWithInteractor()
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, -5);

            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Camera
            GameObject camera = new GameObject("PlayerCamera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 0.5f, 0);
            camera.tag = "MainCamera";
            Camera cam = camera.AddComponent<Camera>();
            camera.AddComponent<AudioListener>();

            // RaycastInteractor
            var interactor = camera.AddComponent<RaycastInteractor>();
            var interactorSerialized = new SerializedObject(interactor);
            interactorSerialized.FindProperty("interactionRange").floatValue = 3f;
            interactorSerialized.FindProperty("interactionKey").intValue = (int)KeyCode.E;
            interactorSerialized.FindProperty("showInteractionPrompt").boolValue = true;
            interactorSerialized.ApplyModifiedProperties();

            return player;
        }

        private static void CreateDoor(Vector3 position)
        {
            GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "Door";
            door.transform.position = position;
            door.transform.localScale = new Vector3(1, 2, 0.1f);

            // Material (blue)
            var renderer = door.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.blue;
            renderer.material = material;

            Rigidbody rb = door.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

            // DoorController
            var doorController = door.AddComponent<DoorController>();
            var doorSerialized = new SerializedObject(doorController);
            doorSerialized.FindProperty("doorType").enumValueIndex = 0; // Hinged
            doorSerialized.FindProperty("openAngle").floatValue = 90f;
            doorSerialized.FindProperty("openSpeed").floatValue = 2f;
            doorSerialized.FindProperty("autoClose").boolValue = false;
            doorSerialized.FindProperty("isLocked").boolValue = false;
            doorSerialized.ApplyModifiedProperties();

            // HighlightSystem
            var highlight = door.AddComponent<HighlightSystem>();
            var highlightSerialized = new SerializedObject(highlight);
            highlightSerialized.FindProperty("highlightMode").enumValueIndex = 0; // Emission
            highlightSerialized.FindProperty("emissionColor").colorValue = Color.yellow;
            highlightSerialized.FindProperty("emissionIntensity").floatValue = 2f;
            highlightSerialized.ApplyModifiedProperties();
        }

        private static void CreateExamineObject(Vector3 position)
        {
            GameObject key = GameObject.CreatePrimitive(PrimitiveType.Cube);
            key.name = "Key";
            key.transform.position = position;
            key.transform.localScale = new Vector3(0.1f, 0.3f, 0.05f);

            // Material (yellow/gold)
            var renderer = key.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.yellow;
            material.SetColor("_EmissionColor", Color.yellow * 0.5f);
            material.EnableKeyword("_EMISSION");
            renderer.material = material;

            Rigidbody rb = key.AddComponent<Rigidbody>();
            rb.mass = 0.1f;

            // ExamineObject
            var examine = key.AddComponent<ExamineObject>();
            var examineSerialized = new SerializedObject(examine);
            examineSerialized.FindProperty("examinePosition").vector3Value = new Vector3(0, 0, 1);
            examineSerialized.FindProperty("examineDistance").floatValue = 0.5f;
            examineSerialized.FindProperty("rotateSpeed").floatValue = 100f;
            examineSerialized.FindProperty("canRotate").boolValue = true;
            examineSerialized.FindProperty("canZoom").boolValue = true;
            examineSerialized.FindProperty("exitKey").intValue = (int)KeyCode.Escape;
            examineSerialized.FindProperty("description").stringValue = "A rusty old key. What does it unlock?";
            examineSerialized.ApplyModifiedProperties();

            // HighlightSystem
            var highlight = key.AddComponent<HighlightSystem>();
            var highlightSerialized = new SerializedObject(highlight);
            highlightSerialized.FindProperty("highlightMode").enumValueIndex = 0; // Emission
            highlightSerialized.FindProperty("emissionColor").colorValue = Color.yellow;
            highlightSerialized.FindProperty("emissionIntensity").floatValue = 3f;
            highlightSerialized.ApplyModifiedProperties();
        }

        private static void CreateHidingSpot(Vector3 position)
        {
            // Closet parent
            GameObject closet = new GameObject("Closet");
            closet.transform.position = position;

            // Back wall
            CreateClosetPart(closet, "Back", new Vector3(0, 1, 0), new Vector3(1.5f, 2, 0.1f), Quaternion.identity);
            // Left wall
            CreateClosetPart(closet, "Left", new Vector3(-0.7f, 1, 0.5f), new Vector3(0.1f, 2, 1), Quaternion.identity);
            // Right wall
            CreateClosetPart(closet, "Right", new Vector3(0.7f, 1, 0.5f), new Vector3(0.1f, 2, 1), Quaternion.identity);
            // Top
            CreateClosetPart(closet, "Top", new Vector3(0, 2, 0.5f), new Vector3(1.5f, 0.1f, 1), Quaternion.identity);

            // Hiding trigger
            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "HidingTrigger";
            trigger.transform.SetParent(closet.transform);
            trigger.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
            trigger.transform.localScale = new Vector3(1, 1.8f, 0.8f);
            Object.DestroyImmediate(trigger.GetComponent<Renderer>()); // Invisible
            BoxCollider triggerCollider = trigger.GetComponent<BoxCollider>();
            triggerCollider.isTrigger = true;

            // HidingSpot component
            var hidingSpot = closet.AddComponent<HidingSpot>();
            var hidingSerialized = new SerializedObject(hidingSpot);
            hidingSerialized.FindProperty("hidingTransform").objectReferenceValue = trigger.transform;
            hidingSerialized.FindProperty("enterKey").intValue = (int)KeyCode.E;
            hidingSerialized.FindProperty("exitKey").intValue = (int)KeyCode.E;
            hidingSerialized.FindProperty("hideCamera").boolValue = true;
            hidingSerialized.FindProperty("cameraHidingPosition").vector3Value = new Vector3(0, 1.6f, 0.3f);
            hidingSerialized.FindProperty("disablePlayerMovement").boolValue = true;
            hidingSerialized.ApplyModifiedProperties();

            Debug.Log("[HidingSpot Created] Manually link: Camera Transform and Movement Script in Inspector");
        }

        private static void CreateClosetPart(GameObject parent, string name, Vector3 localPos, Vector3 scale, Quaternion rotation)
        {
            GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = name;
            part.transform.SetParent(parent.transform);
            part.transform.localPosition = localPos;
            part.transform.localScale = scale;
            part.transform.localRotation = rotation;
            part.isStatic = true;

            // Brown wood material
            var renderer = part.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = new Color(0.4f, 0.2f, 0.1f); // Brown
            renderer.material = material;
        }
    }
}
