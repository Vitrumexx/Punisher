using UnityEngine;

public class OpenShop : MonoBehaviour
{
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

            if (ShopIsActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            ShopIsActive = false;
            ShopCanvas.enabled = false;
        }
    }
}
