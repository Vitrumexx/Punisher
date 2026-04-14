using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    private bool initialized;

    private GameObject persistentPlayer;
    private GameObject persistentCamera;

    // Roots we DDOL'd (to detect duplicates in new scenes by name)
    private HashSet<GameObject> persistentRoots = new HashSet<GameObject>();
    private HashSet<string> persistentRootNames = new HashSet<string>();

    public static SceneTransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("SceneTransitionManager");
                instance = go.AddComponent<SceneTransitionManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void TransitionTo(string sceneName)
    {
        if (!initialized)
            InitializePersistentObjects();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// DontDestroyOnLoad the ROOT of the given GameObject.
    /// Works regardless of hierarchy depth.
    /// </summary>
    private void MakeRootPersistent(GameObject go)
    {
        if (go == null) return;
        GameObject root = go.transform.root.gameObject;
        if (persistentRoots.Contains(root)) return;
        persistentRoots.Add(root);
        persistentRootNames.Add(root.name);
        DontDestroyOnLoad(root);
    }

    private void InitializePersistentObjects()
    {
        // Player
        persistentPlayer = GameObject.FindGameObjectWithTag("Player");
        MakeRootPersistent(persistentPlayer);

        // Camera
        if (Camera.main != null)
        {
            persistentCamera = Camera.main.gameObject;
            MakeRootPersistent(persistentCamera);
        }

        // MenuHandler lives on the Canvas inside the Canvases prefab, NOT on the Player.
        // Find it anywhere in the scene and DDOL its root (the Canvases container).
        MenuHandler mh = FindObjectOfType<MenuHandler>();
        if (mh != null)
        {
            MakeRootPersistent(mh.gameObject);
            if (mh._UI != null) MakeRootPersistent(mh._UI.gameObject);
            if (mh._Parameters != null) MakeRootPersistent(mh._Parameters.gameObject);
            if (mh._GUI != null) MakeRootPersistent(mh._GUI.gameObject);
        }

        // EventSystem (needed for UI interaction / drag-and-drop)
        var es = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (es != null)
            MakeRootPersistent(es.gameObject);

        initialized = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Destroy duplicate root objects that came from the newly loaded scene.
        // Our persistent objects live in the DontDestroyOnLoad scene,
        // so scene.GetRootGameObjects() returns ONLY the new scene's objects.
        foreach (var rootGO in scene.GetRootGameObjects())
        {
            // Match by tag first (most reliable)
            if (rootGO.CompareTag("Player") || rootGO.CompareTag("MainCamera"))
            {
                Destroy(rootGO);
                continue;
            }

            // Match EventSystem by component
            if (rootGO.GetComponent<UnityEngine.EventSystems.EventSystem>() != null)
            {
                Destroy(rootGO);
                continue;
            }

            // Match any other persistent root by name
            if (persistentRootNames.Contains(rootGO.name))
            {
                Destroy(rootGO);
                continue;
            }
        }

        // Reposition player at spawn point (tag "Respawn" in each scene)
        if (persistentPlayer != null)
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
            if (spawnPoint != null)
            {
                // CharacterController blocks transform.position changes — disable temporarily
                CharacterController cc = persistentPlayer.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                persistentPlayer.transform.position = spawnPoint.transform.position;
                persistentPlayer.transform.rotation = spawnPoint.transform.rotation;

                if (cc != null) cc.enabled = true;
            }
        }
    }
}
