using UnityEngine;
using System.Collections.Generic;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private CameraRotation cam;
    public Canvas _UI;
    public Canvas _Parameters;
    public Canvas _GUI;
    public bool MenuIsOpened;
    void Start()
    {
        MenuIsOpened = false;
        _UI.enabled = false;
        _GUI.enabled = true;
        _Parameters.enabled = true;
    }
    void Update()
    {
        CallUI();
        MenuControls();
    }

    void CallUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _UI.enabled = !_UI.enabled;
            _GUI.enabled = !_GUI.enabled;
            _Parameters.enabled = !_Parameters.enabled;
            MenuIsOpened = !MenuIsOpened;
        } 
    }

    void MenuControls()
    {
        if (MenuIsOpened)
        {
            cam.sensitivity = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            cam.sensitivity = 3f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
