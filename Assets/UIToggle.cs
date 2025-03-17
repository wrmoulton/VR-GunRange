using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggleHandler : MonoBehaviour
{
    public GameObject uiPanel; // Assign your UI Panel in the Inspector
    public InputActionReference uiToggleAction; // Assign the UI Toggle action

    private void OnEnable()
    {
        // Subscribe to the UI Toggle action
        uiToggleAction.action.performed += OnUIToggle;
        uiToggleAction.action.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from the UI Toggle action
        uiToggleAction.action.performed -= OnUIToggle;
        uiToggleAction.action.Disable();
    }

    private void OnUIToggle(InputAction.CallbackContext context)
    {
    uiPanel.SetActive(!uiPanel.activeSelf);

        if (uiPanel.activeSelf)
        {
            Transform playerHead = Camera.main.transform;
            uiPanel.transform.position = playerHead.position + playerHead.forward * 1.5f; // 1.5m in front
            uiPanel.transform.rotation = Quaternion.LookRotation(uiPanel.transform.position - playerHead.position);
        }
    }

}
