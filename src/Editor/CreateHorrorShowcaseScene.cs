using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.AI;
using UnityMechanicsToolkit.Movement;
using UnityMechanicsToolkit.Core;
using UnityMechanicsToolkit.Horror;
using UnityMechanicsToolkit.Camera;
using UnityMechanicsToolkit.AI;
using UnityMechanicsToolkit.Interaction;
using UnityMechanicsToolkit.Combat;

namespace UnityMechanicsToolkit.Editor
{
    /// <summary>
    /// Editor tool to create a complete horror game showcase scene.
    /// Combines all Sprint 1 features: Player, Enemy, Interactions, Horror atmosphere.
    /// Menu: Tools > Unity Mechanics Toolkit > Create Horror Showcase Scene
    /// </summary>
    public static class CreateHorrorShowcaseScene
    {
        [MenuItem("Tools/Unity Mechanics Toolkit/Create Demo Scenes/Horror Showcase (Complete)")]
        public static void Create()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // Environment
            CreateEnvironment();

            // Player (full setup)
            GameObject player = CreateCompletePlayer();

            // Enemy
            GameObject enemy = CreateEnemy(player);

            // Interactables
            CreateInteractables(player);

            // Hiding spots
            CreateHidingSpot(new Vector3(-10, 0, 10));
            CreateHidingSpot(new Vector3(10, 0, -10));

            // Lighting (horror atmosphere)
            CreateHorrorLighting();

            Debug.Log("=== HORROR SHOWCASE SCENE CREATED ===");
            Debug.Log("Features: Complete Player, Patrolling Enemy, Doors, Keys, Hiding Spots");
            Debug.Log("Atmosphere: Dark lighting, sanity system, flashlight with battery");
            Debug.Log("\nMANUAL STEPS REQUIRED:");
            Debug.Log("1. Mark all static objects as Navigation Static");
            Debug.Log("2. Window > AI > Navigation > Bake NavMesh");
            Debug.Log("3. Wire VisionCone events: OnTargetDetected -> AIStateMachine.StartChase");
            Debug.Log("4. Wire HidingSpot: Camera Transform and Movement Script");
            Debug.Log("5. Press Play and survive!");

            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static void CreateEnvironment()
        {
            // Large ground
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(20, 1, 20);
            ground.isStatic = true;

            // Walls (perimeter)
            CreateWall("WallNorth", new Vector3(0, 2.5f, 100), new Vector3(200, 5, 1));
            CreateWall("WallSouth", new Vector3(0, 2.5f, -100), new Vector3(200, 5, 1));
            CreateWall("WallEast", new Vector3(100, 2.5f, 0), new Vector3(1, 5, 200));
            CreateWall("WallWest", new Vector3(-100, 2.5f, 0), new Vector3(1, 5, 200));

            // Interior walls (maze-like)
            CreateWall("Wall1", new Vector3(20, 2.5f, 0), new Vector3(1, 5, 40));
            CreateWall("Wall2", new Vector3(-20, 2.5f, 0), new Vector3(1, 5, 40));
            CreateWall("Wall3", new Vector3(0, 2.5f, 20), new Vector3(40, 5, 1));
            CreateWall("Wall4", new Vector3(0, 2.5f, -20), new Vector3(40, 5, 1));

            // Props (boxes, obstacles)
            CreateProp("Box1", new Vector3(15, 1, 15), new Vector3(2, 2, 2));
            CreateProp("Box2", new Vector3(-15, 1, -15), new Vector3(2, 2, 2));
            CreateProp("Box3", new Vector3(-15, 1, 15), new Vector3(2, 2, 2));
        }

        private static GameObject CreateCompletePlayer()
        {
            // Create player
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0, 1, -30);
            player.tag = "Player";

            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = 0.3f;
            cc.center = new Vector3(0, 1, 0);

            // Core
            var stamina = player.AddComponent<StaminaSystem>();
            var health = player.AddComponent<Health>();
            health.maxHealth = 100f;

            // Movement
            var fpc = player.AddComponent<FirstPersonController>();
            var crouch = player.AddComponent<CrouchBehavior>();
            var sprint = player.AddComponent<SprintBehavior>();
            var lean = player.AddComponent<LeanBehavior>();

            // Combat
            var blockDodge = player.AddComponent<BlockDodge>();

            // Camera
            GameObject camera = new GameObject("PlayerCamera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 0.8f, 0);
            camera.tag = "MainCamera";
            Camera cam = camera.AddComponent<Camera>();
            cam.fieldOfView = 60f;
            camera.AddComponent<AudioListener>();

            // Link camera
            var fpcSerialized = new SerializedObject(fpc);
            fpcSerialized.FindProperty("playerCamera").objectReferenceValue = camera.transform;
            fpcSerialized.ApplyModifiedProperties();

            // Camera effects
            camera.AddComponent<CameraShake>();
            camera.AddComponent<HeadBobbing>();
            camera.AddComponent<LookBehind>();

            // Interactor
            var interactor = camera.AddComponent<RaycastInteractor>();

            // Flashlight
            GameObject flashlight = new GameObject("Flashlight");
            flashlight.transform.SetParent(camera.transform);
            flashlight.transform.localPosition = Vector3.zero;
            Light light = flashlight.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 15f;
            light.spotAngle = 60f;
            light.intensity = 2f;
            var flashlightSystem = flashlight.AddComponent<FlashlightSystem>();
            var fsSerialized = new SerializedObject(flashlightSystem);
            fsSerialized.FindProperty("flashlight").objectReferenceValue = light;
            fsSerialized.ApplyModifiedProperties();

            // Sanity
            var sanity = player.AddComponent<SanitySystem>();
            sanity.maxSanity = 100f;
            sanity.drainInDarkness = true;

            return player;
        }

        private static GameObject CreateEnemy(GameObject player)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.transform.position = new Vector3(30, 1, 0);

            var renderer = enemy.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.red;
            renderer.material = material;

            Rigidbody rb = enemy.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
            agent.speed = 3.5f;

            // AI
            enemy.AddComponent<AIStateMachine>();
            var vision = enemy.AddComponent<VisionCone>();
            enemy.AddComponent<PatrolBehavior>();
            enemy.AddComponent<ChaseBehavior>();

            var health = enemy.AddComponent<Health>();
            health.maxHealth = 50f;

            // Create waypoints
            Vector3[] waypoints = new Vector3[]
            {
                new Vector3(30, 0, 0),
                new Vector3(30, 0, 20),
                new Vector3(10, 0, 20),
                new Vector3(10, 0, 0)
            };

            for (int i = 0; i < waypoints.Length; i++)
            {
                GameObject wp = new GameObject($"EnemyWaypoint{i + 1}");
                wp.transform.position = waypoints[i];
            }

            return enemy;
        }

        private static void CreateInteractables(GameObject player)
        {
            // Door 1
            CreateDoor(new Vector3(0, 1, 10), false);

            // Door 2 (locked)
            CreateDoor(new Vector3(0, 1, -10), true);

            // Key
            CreateKey(new Vector3(20, 1, 20));
        }

        private static void CreateDoor(Vector3 position, bool locked)
        {
            GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = locked ? "LockedDoor" : "Door";
            door.transform.position = position;
            door.transform.localScale = new Vector3(1, 2, 0.1f);

            var renderer = door.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = locked ? Color.red : Color.blue;
            renderer.material = material;

            Rigidbody rb = door.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            var doorController = door.AddComponent<DoorController>();
            var dcSerialized = new SerializedObject(doorController);
            dcSerialized.FindProperty("isLocked").boolValue = locked;
            if (locked)
            {
                dcSerialized.FindProperty("requiredKeyName").stringValue = "Rusty Key";
            }
            dcSerialized.ApplyModifiedProperties();

            door.AddComponent<HighlightSystem>();
        }

        private static void CreateKey(Vector3 position)
        {
            GameObject key = GameObject.CreatePrimitive(PrimitiveType.Cube);
            key.name = "Key";
            key.transform.position = position;
            key.transform.localScale = new Vector3(0.1f, 0.3f, 0.05f);

            var renderer = key.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.yellow;
            renderer.material = material;

            key.AddComponent<ExamineObject>();
            key.AddComponent<HighlightSystem>();
        }

        private static void CreateHidingSpot(Vector3 position)
        {
            GameObject closet = new GameObject("Closet");
            closet.transform.position = position;

            // Simple closet structure
            CreateClosetPart(closet, "Back", Vector3.zero, new Vector3(1.5f, 2, 0.1f));
            CreateClosetPart(closet, "Left", new Vector3(-0.7f, 0, 0.5f), new Vector3(0.1f, 2, 1));
            CreateClosetPart(closet, "Right", new Vector3(0.7f, 0, 0.5f), new Vector3(0.1f, 2, 1));
            CreateClosetPart(closet, "Top", new Vector3(0, 1f, 0.5f), new Vector3(1.5f, 0.1f, 1));

            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "HidingTrigger";
            trigger.transform.SetParent(closet.transform);
            trigger.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
            Object.DestroyImmediate(trigger.GetComponent<Renderer>());
            trigger.GetComponent<BoxCollider>().isTrigger = true;

            closet.AddComponent<HidingSpot>();
        }

        private static void CreateHorrorLighting()
        {
            // Very dim directional light (moonlight)
            GameObject dirLight = new GameObject("Moonlight");
            dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light dirLightComponent = dirLight.AddComponent<Light>();
            dirLightComponent.type = LightType.Directional;
            dirLightComponent.intensity = 0.2f; // Very dark
            dirLightComponent.color = new Color(0.7f, 0.7f, 1f); // Slight blue tint

            // Flickering point lights (ambient horror)
            CreateFlickeringLight(new Vector3(15, 2, 15), Color.red, 5f);
            CreateFlickeringLight(new Vector3(-15, 2, -15), Color.green, 5f);
        }

        private static void CreateFlickeringLight(Vector3 position, Color color, float range)
        {
            GameObject lightObj = new GameObject("FlickeringLight");
            lightObj.transform.position = position;
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.range = range;
            light.intensity = 1f;

            // Simple flicker script (optional)
            // TODO: Add FlickerLight component if exists
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.isStatic = true;

            var renderer = wall.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = new Color(0.3f, 0.3f, 0.3f); // Dark gray
            renderer.material = material;
        }

        private static void CreateProp(string name, Vector3 position, Vector3 scale)
        {
            GameObject prop = GameObject.CreatePrimitive(PrimitiveType.Cube);
            prop.name = name;
            prop.transform.position = position;
            prop.transform.localScale = scale;
            prop.isStatic = true;

            var renderer = prop.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = new Color(0.5f, 0.3f, 0.2f); // Brown
            renderer.material = material;
        }

        private static void CreateClosetPart(GameObject parent, string name, Vector3 localPos, Vector3 scale)
        {
            GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = name;
            part.transform.SetParent(parent.transform);
            part.transform.localPosition = localPos;
            part.transform.localScale = scale;
            part.isStatic = true;

            var renderer = part.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = new Color(0.4f, 0.2f, 0.1f); // Brown wood
            renderer.material = material;
        }
    }
}
