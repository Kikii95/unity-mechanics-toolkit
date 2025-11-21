using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.AI;
using UnityMechanicsToolkit.AI;
using UnityMechanicsToolkit.Combat;

namespace UnityMechanicsToolkit.Editor
{
    /// <summary>
    /// Editor tool to automatically create an AI enemy demo scene.
    /// Menu: Tools > Unity Mechanics Toolkit > Create AI Demo Scene
    /// </summary>
    public static class CreateAIDemoScene
    {
        [MenuItem("Tools/Unity Mechanics Toolkit/Create Demo Scenes/AI Enemy Setup")]
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

            // IMPORTANT: User must bake NavMesh manually
            Debug.LogWarning("[Unity Mechanics Toolkit] NavMesh Setup Required:\n1. Select Ground\n2. Mark as Navigation Static\n3. Window > AI > Navigation > Bake");

            // Create simple player for testing
            GameObject player = CreateSimplePlayer();

            // Create enemy
            GameObject enemy = CreateEnemy();

            // Create waypoints
            CreateWaypoints();

            // Create directional light
            GameObject dirLight = new GameObject("Directional Light");
            dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light dirLightComponent = dirLight.AddComponent<Light>();
            dirLightComponent.type = LightType.Directional;

            // Add some obstacles
            CreateObstacle("Wall1", new Vector3(15, 1, 0), new Vector3(1, 2, 30));
            CreateObstacle("Wall2", new Vector3(-15, 1, 0), new Vector3(1, 2, 30));

            Debug.Log("[Unity Mechanics Toolkit] AI Demo Scene created successfully!");
            Debug.Log("NEXT STEPS:\n1. Mark Ground as Navigation Static\n2. Window > AI > Navigation > Bake\n3. Press Play and walk into enemy vision cone");

            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static GameObject CreateSimplePlayer()
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, -10);

            // Simple movement for testing
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Camera
            GameObject camera = new GameObject("Camera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 0.5f, 0);
            camera.tag = "MainCamera";
            camera.AddComponent<Camera>();
            camera.AddComponent<AudioListener>();

            return player;
        }

        private static GameObject CreateEnemy()
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.transform.position = new Vector3(5, 1, 0);

            // Make enemy red (hostile)
            var renderer = enemy.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.red;
            renderer.material = material;

            // Rigidbody
            Rigidbody rb = enemy.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = true;

            // NavMeshAgent
            NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
            agent.speed = 3.5f;
            agent.angularSpeed = 120f;
            agent.acceleration = 8f;
            agent.stoppingDistance = 0.5f;
            agent.autoBraking = true;

            // AI State Machine
            var stateMachine = enemy.AddComponent<AIStateMachine>();
            var smSerialized = new SerializedObject(stateMachine);
            smSerialized.FindProperty("currentState").enumValueIndex = 1; // Patrol
            smSerialized.ApplyModifiedProperties();

            // Vision Cone
            var vision = enemy.AddComponent<VisionCone>();
            var visionSerialized = new SerializedObject(vision);
            visionSerialized.FindProperty("visionRange").floatValue = 10f;
            visionSerialized.FindProperty("visionAngle").floatValue = 90f;
            visionSerialized.FindProperty("targetTag").stringValue = "Player";
            visionSerialized.FindProperty("detectionInterval").floatValue = 0.2f;
            visionSerialized.ApplyModifiedProperties();

            // Wire events: OnTargetDetected -> StartChase, OnTargetLost -> StartSearch
            // (Must be done manually in Inspector or via code - UnityEvents are complex)

            // Patrol Behavior
            var patrol = enemy.AddComponent<PatrolBehavior>();
            var patrolSerialized = new SerializedObject(patrol);
            patrolSerialized.FindProperty("waypointReachedDistance").floatValue = 0.5f;
            patrolSerialized.FindProperty("waitTimeAtWaypoint").floatValue = 2f;
            patrolSerialized.FindProperty("loopWaypoints").boolValue = true;
            patrolSerialized.FindProperty("patrolSpeed").floatValue = 2f;
            patrolSerialized.ApplyModifiedProperties();

            // Chase Behavior
            var chase = enemy.AddComponent<ChaseBehavior>();
            var chaseSerialized = new SerializedObject(chase);
            chaseSerialized.FindProperty("chaseSpeed").floatValue = 5f;
            chaseSerialized.FindProperty("attackRange").floatValue = 2f;
            chaseSerialized.FindProperty("giveUpDistance").floatValue = 20f;
            chaseSerialized.FindProperty("lostTargetSearchTime").floatValue = 5f;
            chaseSerialized.ApplyModifiedProperties();

            // Health (make killable)
            var health = enemy.AddComponent<Health>();
            health.maxHealth = 50f;

            Debug.Log("[Enemy Created] Don't forget to:\n1. Wire VisionCone events to AIStateMachine (OnTargetDetected -> StartChase, OnTargetLost -> StartSearch)\n2. Assign waypoints to PatrolBehavior");

            return enemy;
        }

        private static void CreateWaypoints()
        {
            Vector3[] positions = new Vector3[]
            {
                new Vector3(5, 0, 0),
                new Vector3(10, 0, 5),
                new Vector3(5, 0, 10),
                new Vector3(0, 0, 5)
            };

            for (int i = 0; i < positions.Length; i++)
            {
                GameObject waypoint = new GameObject($"Waypoint{i + 1}");
                waypoint.transform.position = positions[i];

                // Visual indicator
                Gizmos.DrawWireSphere(positions[i], 0.5f);
            }

            Debug.Log("[Waypoints Created] Drag waypoints into Enemy > PatrolBehavior > Waypoints array");
        }

        private static void CreateObstacle(string name, Vector3 position, Vector3 scale)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.transform.position = position;
            obj.transform.localScale = scale;
            obj.isStatic = true;
        }
    }
}
