using UnityEngine;
using TMPro;

public class InteractionHint : MonoBehaviour
{
    private GameObject hintObject;
    private TextMeshProUGUI label;
    private Camera cam;

    [SerializeField] private float heightOffset = 1.5f;
    [SerializeField] private LayerMask occlusionLayers; // Assign "Wall", "Floor", etc.

    public void Setup(string key)
    {
        if (hintObject != null) return;

        // Canvas lives at scene root so parent rotation doesn't interfere
        hintObject = new GameObject("HintCanvas_" + gameObject.name);

        Canvas canvas = hintObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 100;

        RectTransform canvasRT = hintObject.GetComponent<RectTransform>();
        canvasRT.sizeDelta = new Vector2(200, 60);
        canvasRT.localScale = Vector3.one * 0.02f;

        GameObject textObj = new GameObject("Label");
        textObj.transform.SetParent(hintObject.transform, false);

        label = textObj.AddComponent<TextMeshProUGUI>();
        label.text = $"[{key}]";
        label.fontSize = 42;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;
        label.fontStyle = FontStyles.Bold;
        label.outlineWidth = 0.2f;
        label.outlineColor = Color.black;

        // ZTest Always — render on top of all geometry
        label.materialForRendering.SetInt("_ZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);

        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        hintObject.SetActive(false);
    }

    public void Show()
    {
        if (hintObject == null) Setup("E");
        hintObject.SetActive(true);
    }

    public void Hide()
    {
        if (hintObject != null)
            hintObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (hintObject == null || !hintObject.activeSelf) return;

        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector3 hintWorldPos = transform.position + Vector3.up * heightOffset;

        // Hide if a wall/floor is between camera and hint
        Vector3 toHint = hintWorldPos - cam.transform.position;
        if (occlusionLayers.value != 0 &&
            Physics.Raycast(cam.transform.position, toHint.normalized, toHint.magnitude, occlusionLayers))
        {
            label.enabled = false;
            return;
        }
        label.enabled = true;

        // Follow the object
        hintObject.transform.position = hintWorldPos;

        // Billboard: face toward camera
        Vector3 dirToCamera = cam.transform.position - hintWorldPos;
        dirToCamera.y = 0f;
        if (dirToCamera.sqrMagnitude > 0.001f)
            hintObject.transform.rotation = Quaternion.LookRotation(-dirToCamera);
    }

    private void OnDestroy()
    {
        if (hintObject != null)
            Destroy(hintObject);
    }
}
