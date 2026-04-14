using UnityEngine;
using UnityEditor;
using System.Reflection;

public class WeaponSetupEditor
{
    struct WeaponConfig
    {
        public string prefabPath;
        public string assetDir;
        public string assetName;
        public string displayName;
        public string description;
        public DistantWeaponType weaponType;
        public int clipAmmo;
        public int maxAmmo;
        public float damage;
        public float shootingSpeed;
        public string soundPath;
    }

    [MenuItem("Tools/Setup New Weapons")]
    public static void SetupWeapons()
    {
        // Load reference data from AKM (already configured weapon)
        GameObject akmPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/AKM.prefab");
        if (akmPrefab == null)
        {
            Debug.LogError("AKM prefab not found! Cannot setup weapons.");
            return;
        }

        DistantWeaponLogic akmLogic = akmPrefab.GetComponent<DistantWeaponLogic>();
        if (akmLogic == null)
        {
            Debug.LogError("AKM has no DistantWeaponLogic! Cannot setup weapons.");
            return;
        }

        // Get shared references from AKM
        GameObject projectile = akmLogic.projectile;
        AudioClip emptyClip = akmLogic.emptyClip;

        // Define all weapons to set up
        WeaponConfig[] weapons = new WeaponConfig[]
        {
            new WeaponConfig
            {
                prefabPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG1.prefab",
                assetDir = "Assets/Interactable/Weapon/Guns/Assault Rifles",
                assetName = "MG-1",
                displayName = "MG-1",
                description = "Light machine gun with high rate of fire",
                weaponType = DistantWeaponType.AssaultRifle,
                clipAmmo = 40, maxAmmo = 800, damage = 12, shootingSpeed = 12,
                soundPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Sounds/gun-shot.mp3"
            },
            new WeaponConfig
            {
                prefabPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG2.prefab",
                assetDir = "Assets/Interactable/Weapon/Guns/Assault Rifles",
                assetName = "MG-2",
                displayName = "MG-2",
                description = "Medium machine gun with balanced stats",
                weaponType = DistantWeaponType.AssaultRifle,
                clipAmmo = 50, maxAmmo = 900, damage = 14, shootingSpeed = 11,
                soundPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Sounds/gun-shot.mp3"
            },
            new WeaponConfig
            {
                prefabPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG3.prefab",
                assetDir = "Assets/Interactable/Weapon/Guns/Assault Rifles",
                assetName = "MG-3",
                displayName = "MG-3",
                description = "Heavy machine gun with high damage output",
                weaponType = DistantWeaponType.AssaultRifle,
                clipAmmo = 60, maxAmmo = 1000, damage = 16, shootingSpeed = 10,
                soundPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Sounds/m4a1-s.mp3"
            },
            new WeaponConfig
            {
                prefabPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG4.prefab",
                assetDir = "Assets/Interactable/Weapon/Guns/Assault Rifles",
                assetName = "MG-4",
                displayName = "MG-4",
                description = "Advanced machine gun with large magazine",
                weaponType = DistantWeaponType.AssaultRifle,
                clipAmmo = 75, maxAmmo = 1200, damage = 18, shootingSpeed = 9,
                soundPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Sounds/m4a1-s.mp3"
            },
            new WeaponConfig
            {
                prefabPath = "", // AugA1 needs special handling (FBX only)
                assetDir = "Assets/Interactable/Weapon/Guns/Assault Rifles",
                assetName = "AugA1",
                displayName = "AUG A1",
                description = "Accurate assault rifle with integrated optics",
                weaponType = DistantWeaponType.AssaultRifle,
                clipAmmo = 30, maxAmmo = 600, damage = 28, shootingSpeed = 11,
                soundPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Sounds/m4a1-s.mp3"
            },
            new WeaponConfig
            {
                prefabPath = "Assets/Interactable/Weapon/Guns/Rifles/SNIPER1.prefab",
                assetDir = "Assets/Interactable/Weapon/Guns/Rifles",
                assetName = "SNIPER1",
                displayName = "Sniper Rifle",
                description = "High-powered sniper rifle with extreme damage",
                weaponType = DistantWeaponType.Rifle,
                clipAmmo = 5, maxAmmo = 50, damage = 80, shootingSpeed = 1,
                soundPath = "Assets/Interactable/Weapon/Guns/Pistols/Sounds/gunshot.mp3"
            }
        };

        int successCount = 0;

        foreach (var config in weapons)
        {
            string assetPath = config.assetDir + "/" + config.assetName + ".asset";

            // Skip if asset already exists
            if (AssetDatabase.LoadAssetAtPath<DistantWeapon>(assetPath) != null)
            {
                Debug.Log($"[WeaponSetup] Skipping {config.displayName} - asset already exists at {assetPath}");
                continue;
            }

            string prefabPath = config.prefabPath;

            // Special handling for AugA1 - create prefab from FBX
            if (string.IsNullOrEmpty(prefabPath))
            {
                prefabPath = CreatePrefabFromFBX(
                    "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/AugA1.fbx",
                    "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/AugA1.prefab");
                if (string.IsNullOrEmpty(prefabPath))
                {
                    Debug.LogError($"[WeaponSetup] Failed to create prefab for {config.displayName}");
                    continue;
                }
            }

            // Setup prefab components
            if (!SetupPrefabComponents(prefabPath, projectile, emptyClip, config.soundPath))
            {
                Debug.LogError($"[WeaponSetup] Failed to setup prefab for {config.displayName}");
                continue;
            }

            // Create ScriptableObject asset
            GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefabObj == null)
            {
                Debug.LogError($"[WeaponSetup] Cannot load prefab at {prefabPath}");
                continue;
            }

            DistantWeapon weaponAsset = ScriptableObject.CreateInstance<DistantWeapon>();
            weaponAsset.Name = config.displayName;
            weaponAsset.Description = config.description;
            weaponAsset.maxAmount = 1;
            weaponAsset.Type = ItemType.Weapon;
            weaponAsset.isConsumeable = false;
            weaponAsset.WeaponType = config.weaponType;
            weaponAsset.clipAmmo = config.clipAmmo;
            weaponAsset.maxAmmo = config.maxAmmo;
            weaponAsset.damage = config.damage;
            weaponAsset.shootingSpeed = config.shootingSpeed;
            weaponAsset.itemPrefab = prefabObj;

            AssetDatabase.CreateAsset(weaponAsset, assetPath);
            Debug.Log($"[WeaponSetup] Created asset: {assetPath}");

            // Link Item component on prefab to the created asset
            LinkItemToAsset(prefabPath, assetPath);

            successCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[WeaponSetup] Done! Successfully set up {successCount} weapons.");
    }

    static string CreatePrefabFromFBX(string fbxPath, string prefabPath)
    {
        // Check if prefab already exists
        if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
            return prefabPath;

        GameObject fbxModel = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
        if (fbxModel == null)
        {
            Debug.LogError($"[WeaponSetup] FBX not found at {fbxPath}");
            return null;
        }

        GameObject instance = Object.Instantiate(fbxModel);
        instance.name = "AugA1";

        GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
        Object.DestroyImmediate(instance);

        if (savedPrefab == null)
        {
            Debug.LogError($"[WeaponSetup] Failed to save prefab at {prefabPath}");
            return null;
        }

        Debug.Log($"[WeaponSetup] Created prefab from FBX: {prefabPath}");
        return prefabPath;
    }

    static bool SetupPrefabComponents(string prefabPath, GameObject projectile, AudioClip emptyClip, string soundPath)
    {
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
        if (prefabRoot == null)
        {
            Debug.LogError($"[WeaponSetup] Cannot load prefab contents: {prefabPath}");
            return false;
        }

        // Set layer to 3 (same as AKM)
        SetLayerRecursive(prefabRoot, 3);

        // Normalize scale to match AKM size using mesh bounds
        GameObject akmRef = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/AKM.prefab");
        float akmLen = akmRef != null ? GetMeshLength(akmRef) : -1f;

        if (akmLen > 0f)
        {
            prefabRoot.transform.localScale = Vector3.one;
            float rawLen = GetMeshLengthFromContents(prefabRoot);
            if (rawLen > 0f)
            {
                float targetScale = akmLen / rawLen;
                prefabRoot.transform.localScale = Vector3.one * targetScale;
            }
        }
        prefabRoot.transform.localRotation = Quaternion.identity;
        prefabRoot.transform.localPosition = Vector3.zero;

        // Add Rigidbody
        if (prefabRoot.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = prefabRoot.AddComponent<Rigidbody>();
            rb.mass = 1f;
            rb.angularDrag = 0.05f;
        }

        // Add BoxCollider
        if (prefabRoot.GetComponent<BoxCollider>() == null)
        {
            BoxCollider col = prefabRoot.AddComponent<BoxCollider>();
            // Auto-fit from renderers
            Renderer rend = prefabRoot.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                Bounds bounds = rend.bounds;
                col.center = prefabRoot.transform.InverseTransformPoint(bounds.center);
                col.size = bounds.size;
                // Scale down to local space
                Vector3 lossyScale = prefabRoot.transform.lossyScale;
                if (lossyScale.x != 0) col.size = new Vector3(col.size.x / lossyScale.x, col.size.y / lossyScale.y, col.size.z / lossyScale.z);
            }
        }

        // Add AudioSource
        AudioSource audioSource = prefabRoot.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = prefabRoot.AddComponent<AudioSource>();
        }
        AudioClip shotClip = AssetDatabase.LoadAssetAtPath<AudioClip>(soundPath);
        audioSource.clip = shotClip;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.maxDistance = 500f;

        // Create GripPoint child
        Transform gripPoint = prefabRoot.transform.Find("GripPoint");
        if (gripPoint == null)
        {
            GameObject gripObj = new GameObject("GripPoint");
            gripObj.layer = 3;
            gripObj.transform.SetParent(prefabRoot.transform, false);
            gripObj.transform.localPosition = new Vector3(1.79f, -0.19f, 0f);
            gripObj.transform.localRotation = Quaternion.identity;
            gripObj.transform.localScale = Vector3.one * 2.5f;
        }

        // Create projSpawnPoint child
        Transform projSpawn = prefabRoot.transform.Find("projSpawnPoint");
        if (projSpawn == null)
        {
            GameObject projSpawnObj = new GameObject("projSpawnPoint");
            projSpawnObj.layer = 3;
            projSpawnObj.transform.SetParent(prefabRoot.transform, false);
            projSpawnObj.transform.localPosition = new Vector3(-4.8f, 0.707f, 0f);
            projSpawnObj.transform.localRotation = Quaternion.Euler(0f, -90f, 90f);
            projSpawn = projSpawnObj.transform;
        }

        // Add DistantWeaponLogic
        DistantWeaponLogic logic = prefabRoot.GetComponent<DistantWeaponLogic>();
        if (logic == null)
        {
            logic = prefabRoot.AddComponent<DistantWeaponLogic>();
        }
        logic.projectile = projectile;
        logic.emptyClip = emptyClip;

        // Set projSpawnPoint via SerializedObject (it's a private serialized field)
        SerializedObject so = new SerializedObject(logic);
        SerializedProperty projSpawnProp = so.FindProperty("projSpawnPoint");
        if (projSpawnProp != null)
        {
            projSpawnProp.objectReferenceValue = projSpawn;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // Add Item component
        if (prefabRoot.GetComponent<Item>() == null)
        {
            Item item = prefabRoot.AddComponent<Item>();
            item.amount = 1;
        }

        // Save prefab
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        Debug.Log($"[WeaponSetup] Configured prefab: {prefabPath}");
        return true;
    }

    static void LinkItemToAsset(string prefabPath, string assetPath)
    {
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
        if (prefabRoot == null) return;

        Item item = prefabRoot.GetComponent<Item>();
        DistantWeapon weaponAsset = AssetDatabase.LoadAssetAtPath<DistantWeapon>(assetPath);

        if (item != null && weaponAsset != null)
        {
            item.item = weaponAsset;
            item.amount = 1;
        }

        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
    }

    [MenuItem("Tools/Fix Weapon Scales")]
    public static void FixWeaponScales()
    {
        // Use AKM as the reference — its size is correct
        string akmPath = "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/AKM.prefab";
        GameObject akmPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(akmPath);
        if (akmPrefab == null)
        {
            Debug.LogError("[FixScale] AKM prefab not found!");
            return;
        }

        float akmLength = GetMeshLength(akmPrefab);
        if (akmLength <= 0f)
        {
            Debug.LogError("[FixScale] Cannot measure AKM bounds!");
            return;
        }
        Debug.Log($"[FixScale] AKM reference length: {akmLength}");

        string[] prefabPaths = new string[]
        {
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG2.prefab",
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG3.prefab",
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/MG4.prefab",
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/M4A1.prefab",
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/AugA1.prefab",
            "Assets/Interactable/Weapon/Guns/Assault Rifles/Models/SCAR-L.prefab",
            "Assets/Interactable/Weapon/Guns/Rifles/SNIPER1.prefab",
            "Assets/Interactable/Weapon/Guns/Explosive/Models/GL1.prefab",
            "Assets/Interactable/Weapon/Guns/Explosive/Models/RPG.prefab",
        };

        foreach (string path in prefabPaths)
        {
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
            if (prefabRoot == null)
            {
                Debug.LogWarning($"[FixScale] Prefab not found: {path}");
                continue;
            }

            // Temporarily set scale to 1 to measure raw mesh size
            Vector3 oldScale = prefabRoot.transform.localScale;
            prefabRoot.transform.localScale = Vector3.one;

            float rawLength = GetMeshLengthFromContents(prefabRoot);
            if (rawLength <= 0f)
            {
                Debug.LogWarning($"[FixScale] Cannot measure bounds for {path}, skipping");
                PrefabUtility.UnloadPrefabContents(prefabRoot);
                continue;
            }

            // Calculate scale so this weapon matches AKM's length
            float targetScale = akmLength / rawLength;
            prefabRoot.transform.localScale = Vector3.one * targetScale;
            prefabRoot.transform.localRotation = Quaternion.identity;
            prefabRoot.transform.localPosition = Vector3.zero;

            Debug.Log($"[FixScale] {path}: rawLength={rawLength:F4}, scale {oldScale} -> ({targetScale:F4}, {targetScale:F4}, {targetScale:F4})");

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("[FixScale] Done!");
    }

    /// <summary>
    /// Get the longest axis of the combined mesh bounds for a prefab asset (not loaded contents).
    /// Uses the prefab's current scale.
    /// </summary>
    static float GetMeshLength(GameObject prefab)
    {
        MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>();
        SkinnedMeshRenderer[] skinned = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (filters.Length == 0 && skinned.Length == 0) return -1f;

        Bounds combined = new Bounds();
        bool first = true;

        foreach (var mf in filters)
        {
            if (mf.sharedMesh == null) continue;
            Bounds b = mf.sharedMesh.bounds;
            // Transform to prefab root space
            Vector3 center = mf.transform.TransformPoint(b.center);
            center = prefab.transform.InverseTransformPoint(center);
            Vector3 size = Vector3.Scale(b.size, mf.transform.lossyScale);

            if (first) { combined = new Bounds(center, size); first = false; }
            else combined.Encapsulate(new Bounds(center, size));
        }

        foreach (var smr in skinned)
        {
            Bounds b = smr.sharedMesh != null ? smr.sharedMesh.bounds : smr.localBounds;
            Vector3 center = smr.transform.TransformPoint(b.center);
            center = prefab.transform.InverseTransformPoint(center);
            Vector3 size = Vector3.Scale(b.size, smr.transform.lossyScale);

            if (first) { combined = new Bounds(center, size); first = false; }
            else combined.Encapsulate(new Bounds(center, size));
        }

        Vector3 scaledSize = Vector3.Scale(combined.size, prefab.transform.localScale);
        return Mathf.Max(scaledSize.x, scaledSize.y, scaledSize.z);
    }

    /// <summary>
    /// Same as GetMeshLength but for already-loaded prefab contents (instantiated root).
    /// Uses current localScale of the root.
    /// </summary>
    static float GetMeshLengthFromContents(GameObject root)
    {
        MeshFilter[] filters = root.GetComponentsInChildren<MeshFilter>();
        SkinnedMeshRenderer[] skinned = root.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (filters.Length == 0 && skinned.Length == 0) return -1f;

        Bounds combined = new Bounds();
        bool first = true;

        foreach (var mf in filters)
        {
            if (mf.sharedMesh == null) continue;
            Bounds b = mf.sharedMesh.bounds;
            if (first) { combined = b; first = false; }
            else combined.Encapsulate(b);
        }

        foreach (var smr in skinned)
        {
            Bounds b = smr.sharedMesh != null ? smr.sharedMesh.bounds : smr.localBounds;
            if (first) { combined = b; first = false; }
            else combined.Encapsulate(b);
        }

        return Mathf.Max(combined.size.x, combined.size.y, combined.size.z);
    }

    static void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
