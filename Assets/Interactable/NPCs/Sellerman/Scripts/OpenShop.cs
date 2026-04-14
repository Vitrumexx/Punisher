using UnityEngine;

public class OpenShop : MonoBehaviour
{
    [SerializeField] private CameraRotation cam;
    public MenuHandler menu;
    public Canvas ShopCanvas;
    public bool ShopIsActive;
    private bool playerIsNear = false;

    private void Start()
    {
        ShopCanvas.gameObject.GetComponent<Canvas>().enabled = false;
        ShopIsActive = false;
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            ShopIsActive = !ShopIsActive;
            ShopCanvas.enabled = ShopIsActive;

            InteractionHint hint = GetComponent<InteractionHint>();
            if (hint != null)
            {
                if (ShopIsActive) hint.Hide();
                else hint.Show();
            }

            if (ShopIsActive)
            {
                cam.sensitivity = 0f;
                menu.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                cam.sensitivity = 3f;
                menu.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;

            InteractionHint hint = GetComponent<InteractionHint>();
            if (hint == null)
            {
                hint = gameObject.AddComponent<InteractionHint>();
                hint.Setup("E");
            }
            hint.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            ShopIsActive = false;
            ShopCanvas.enabled = false;

            InteractionHint hint = GetComponent<InteractionHint>();
            if (hint != null) hint.Hide();
        }
    }
}
