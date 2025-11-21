using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityMechanicsToolkit.Combat;
using UnityMechanicsToolkit.Core;

namespace UnityMechanicsToolkit.Editor
{
    /// <summary>
    /// Editor tool to automatically create a combat systems demo scene.
    /// Menu: Tools > Unity Mechanics Toolkit > Create Combat Demo Scene
    /// </summary>
    public static class CreateCombatDemoScene
    {
        [MenuItem("Tools/Unity Mechanics Toolkit/Create Demo Scenes/Combat Systems")]
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

            // Create player with combat
            GameObject player = CreatePlayerWithCombat();

            // Create dummy targets
            CreateDummyTarget(new Vector3(3, 1, 0), "Dummy1");
            CreateDummyTarget(new Vector3(-3, 1, 0), "Dummy2");
            CreateDummyTarget(new Vector3(0, 1, 5), "Dummy3");

            // Create light
            GameObject dirLight = new GameObject("Directional Light");
            dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light dirLightComponent = dirLight.AddComponent<Light>();
            dirLightComponent.type = LightType.Directional;

            Debug.Log("[Unity Mechanics Toolkit] Combat Demo Scene created successfully!");
            Debug.Log("Controls: Left Click = Attack, Right Click = Block, Left Ctrl = Dodge");

            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static GameObject CreatePlayerWithCombat()
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, -5);

            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = 0.5f;

            // Camera
            GameObject camera = new GameObject("PlayerCamera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 0.5f, 0);
            camera.tag = "MainCamera";
            camera.AddComponent<Camera>();
            camera.AddComponent<AudioListener>();

            // Stamina System
            var stamina = player.AddComponent<StaminaSystem>();
            stamina.maxStamina = 100f;
            stamina.regenRate = 10f;

            // Health
            var health = player.AddComponent<Health>();
            health.maxHealth = 100f;

            // BlockDodge
            var blockDodge = player.AddComponent<BlockDodge>();
            var bdSerialized = new SerializedObject(blockDodge);
            bdSerialized.FindProperty("blockKey").intValue = (int)KeyCode.Mouse1;
            bdSerialized.FindProperty("blockDamageReduction").floatValue = 0.5f;
            bdSerialized.FindProperty("blockStaminaDrain").floatValue = 10f;
            bdSerialized.FindProperty("dodgeKey").intValue = (int)KeyCode.LeftControl;
            bdSerialized.FindProperty("dodgeDistance").floatValue = 3f;
            bdSerialized.FindProperty("dodgeDuration").floatValue = 0.5f;
            bdSerialized.FindProperty("dodgeCooldown").floatValue = 1f;
            bdSerialized.FindProperty("dodgeStaminaCost").floatValue = 20f;
            bdSerialized.ApplyModifiedProperties();

            // Create weapon
            CreateMeleeWeapon(player);

            return player;
        }

        private static void CreateMeleeWeapon(GameObject player)
        {
            GameObject weapon = GameObject.CreatePrimitive(PrimitiveType.Cube);
            weapon.name = "Bat";
            weapon.transform.SetParent(player.transform);
            weapon.transform.localPosition = new Vector3(0.3f, 0, 0.5f);
            weapon.transform.localRotation = Quaternion.Euler(0, 0, 45);
            weapon.transform.localScale = new Vector3(0.1f, 1f, 0.1f);

            // Material (gray)
            var renderer = weapon.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.gray;
            renderer.material = material;

            // Collider as trigger
            BoxCollider weaponCollider = weapon.GetComponent<BoxCollider>();
            weaponCollider.isTrigger = true;

            // MeleeWeapon component
            var melee = weapon.AddComponent<MeleeWeapon>();
            var meleeSerialized = new SerializedObject(melee);
            meleeSerialized.FindProperty("attackKey").intValue = (int)KeyCode.Mouse0;
            meleeSerialized.FindProperty("damage").floatValue = 25f;
            meleeSerialized.FindProperty("attackRange").floatValue = 2f;
            meleeSerialized.FindProperty("attackCooldown").floatValue = 1f;
            meleeSerialized.FindProperty("staminaCost").floatValue = 15f;
            meleeSerialized.FindProperty("maxDurability").floatValue = 100f;
            meleeSerialized.FindProperty("durabilityLossPerHit").floatValue = 5f;
            meleeSerialized.ApplyModifiedProperties();

            Debug.Log("[Weapon Created] Bat equipped to player");
        }

        private static void CreateDummyTarget(Vector3 position, string name)
        {
            GameObject dummy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            dummy.name = name;
            dummy.transform.position = position;

            // Material (red)
            var renderer = dummy.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.red;
            renderer.material = material;

            // Health
            var health = dummy.AddComponent<Health>();
            health.maxHealth = 50f;

            // Add text label above dummy
            GameObject label = new GameObject($"{name}_Label");
            label.transform.SetParent(dummy.transform);
            label.transform.localPosition = new Vector3(0, 1.5f, 0);

            // Simple label using TextMesh (basic)
            TextMesh textMesh = label.AddComponent<TextMesh>();
            textMesh.text = $"{name}\nHP: {health.maxHealth}";
            textMesh.fontSize = 30;
            textMesh.color = Color.white;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;

            Debug.Log($"[Dummy Created] {name} with {health.maxHealth} HP");
        }
    }
}
