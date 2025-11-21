using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityMechanicsToolkit.Movement;
using UnityMechanicsToolkit.Core;
using UnityMechanicsToolkit.Horror;
using UnityMechanicsToolkit.Camera;

namespace UnityMechanicsToolkit.Editor
{
    /// <summary>
    /// Editor tool to automatically create a complete player demo scene.
    /// Menu: Tools > Unity Mechanics Toolkit > Create Player Demo Scene
    /// </summary>
    public static class CreatePlayerDemoScene
    {
        [MenuItem("Tools/Unity Mechanics Toolkit/Create Demo Scenes/Player Complete Setup")]
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

            // Create player
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0, 1, 0);
            player.tag = "Player";

            // Add CharacterController
            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = 0.3f;
            cc.center = new Vector3(0, 1, 0);

            // Add Core Systems
            var stamina = player.AddComponent<StaminaSystem>();
            stamina.maxStamina = 100f;
            stamina.regenRate = 10f;
            stamina.regenDelay = 2f;

            // Add Movement Components
            var fpc = player.AddComponent<FirstPersonController>();
            fpc.moveSpeed = 5f;
            fpc.sprintMultiplier = 1.5f;
            fpc.jumpForce = 8f;
            fpc.mouseSensitivity = 2f;

            var crouch = player.AddComponent<CrouchBehavior>();
            crouch.crouchKey = KeyCode.C;
            crouch.crouchHeight = 1f;
            crouch.standingHeight = 2f;

            var sprint = player.AddComponent<SprintBehavior>();
            sprint.sprintKey = KeyCode.LeftShift;
            sprint.sprintSpeedMultiplier = 1.5f;
            sprint.useStamina = true;
            sprint.staminaDrainRate = 15f;
            sprint.changeFOV = true;
            sprint.sprintFOV = 75f;
            sprint.normalFOV = 60f;

            var lean = player.AddComponent<LeanBehavior>();
            lean.leanLeftKey = KeyCode.A;  // AZERTY
            lean.leanRightKey = KeyCode.E; // AZERTY
            lean.leanAngle = 15f;
            lean.leanDistance = 0.5f;

            // Create camera
            GameObject camera = new GameObject("PlayerCamera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 0.8f, 0);
            camera.tag = "MainCamera";

            Camera cam = camera.AddComponent<Camera>();
            cam.fieldOfView = 60f;
            camera.AddComponent<AudioListener>();

            // Link camera to components
            var fpcSerialized = new SerializedObject(fpc);
            fpcSerialized.FindProperty("playerCamera").objectReferenceValue = camera.transform;
            fpcSerialized.ApplyModifiedProperties();

            var crouchSerialized = new SerializedObject(crouch);
            crouchSerialized.FindProperty("cameraTransform").objectReferenceValue = camera.transform;
            crouchSerialized.FindProperty("cameraStandingY").floatValue = 0.8f;
            crouchSerialized.FindProperty("cameraCrouchY").floatValue = 0.3f;
            crouchSerialized.ApplyModifiedProperties();

            var sprintSerialized = new SerializedObject(sprint);
            sprintSerialized.FindProperty("playerCamera").objectReferenceValue = cam;
            sprintSerialized.ApplyModifiedProperties();

            var leanSerialized = new SerializedObject(lean);
            leanSerialized.FindProperty("cameraTransform").objectReferenceValue = camera.transform;
            leanSerialized.ApplyModifiedProperties();

            // Add Camera Effects
            var shake = camera.AddComponent<CameraShake>();
            shake.defaultIntensity = 0.3f;
            shake.defaultDuration = 0.5f;

            var headBob = camera.AddComponent<HeadBobbing>();
            headBob.enableBobbing = true;
            headBob.walkBobbingSpeed = 14f;
            headBob.walkBobbingAmount = 0.05f;

            var lookBehind = camera.AddComponent<LookBehind>();
            lookBehind.lookBehindKey = KeyCode.V;
            lookBehind.holdToLook = true;

            // Create flashlight
            GameObject flashlight = new GameObject("Flashlight");
            flashlight.transform.SetParent(camera.transform);
            flashlight.transform.localPosition = Vector3.zero;
            flashlight.transform.localRotation = Quaternion.identity;

            Light light = flashlight.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 15f;
            light.spotAngle = 60f;
            light.intensity = 2f;

            var flashlightSystem = flashlight.AddComponent<FlashlightSystem>();
            var flashlightSerialized = new SerializedObject(flashlightSystem);
            flashlightSerialized.FindProperty("flashlight").objectReferenceValue = light;
            flashlightSerialized.FindProperty("toggleKey").intValue = (int)KeyCode.F;
            flashlightSerialized.FindProperty("maxBattery").floatValue = 100f;
            flashlightSerialized.FindProperty("drainRate").floatValue = 5f;
            flashlightSerialized.FindProperty("flickerWhenLow").boolValue = true;
            flashlightSerialized.ApplyModifiedProperties();

            // Add Sanity System
            var sanity = player.AddComponent<SanitySystem>();
            sanity.maxSanity = 100f;
            sanity.drainInDarkness = true;
            sanity.darknessDrainRate = 2f;
            sanity.canRecover = true;
            sanity.recoveryRate = 1f;

            // Create directional light
            GameObject dirLight = new GameObject("Directional Light");
            dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light dirLightComponent = dirLight.AddComponent<Light>();
            dirLightComponent.type = LightType.Directional;
            dirLightComponent.intensity = 0.5f; // Dim for horror atmosphere

            // Add some obstacles for testing
            CreateTestObstacle("Wall1", new Vector3(10, 1, 0), new Vector3(1, 2, 20));
            CreateTestObstacle("Wall2", new Vector3(-10, 1, 0), new Vector3(1, 2, 20));
            CreateTestObstacle("Box1", new Vector3(5, 0.5f, 5), new Vector3(1, 1, 1));
            CreateTestObstacle("Box2", new Vector3(-5, 0.5f, -5), new Vector3(1, 1, 1));

            Debug.Log("[Unity Mechanics Toolkit] Player Demo Scene created successfully!");
            Debug.Log("Controls: ZQSD (AZERTY) / WASD (QWERTY), Mouse, Space=Jump, C=Crouch, Shift=Sprint, A/E=Lean, F=Flashlight, V=LookBehind");

            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static void CreateTestObstacle(string name, Vector3 position, Vector3 scale)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.transform.position = position;
            obj.transform.localScale = scale;
            obj.isStatic = true;
        }
    }
}
