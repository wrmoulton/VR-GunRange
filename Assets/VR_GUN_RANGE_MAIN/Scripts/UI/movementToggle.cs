using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using BNG;

public class movementToggle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Toggle teleportationToggle;
    [SerializeField] private Toggle smoothLocomotionToggle;
    [SerializeField] private GameObject playerController;

    private SmoothLocomotion smoothLocomotion;
    private PlayerTeleport playerTeleport;
    void Start()
    {
        if (playerController != null)
        {
            smoothLocomotion = playerController.GetComponent<SmoothLocomotion>();
            playerTeleport = playerController.GetComponent<PlayerTeleport>();
            // Ensure the initial movement mode matches the toggle selections
            OnTeleportationToggleChanged(teleportationToggle.isOn);

        }
        else
        {
            Debug.LogError("PlayerController is not assigned.");
        }

    }

    void Update()
    {
        // not needed
    }
    public void OnTeleportationToggleChanged(bool isOn)
    {
        if (isOn)
        {
            switchMovementScheme(true); // Enable Teleportation, Disable Smooth Locomotion
        }
    }

    public void OnSmoothLocomotionToggleChanged(bool isOn)
    {
        if (isOn)
        {
            switchMovementScheme(false); // Enable Smooth Locomotion, Disable Teleportation
        }
    }

    private void switchMovementScheme(bool useTeleportation)
    {
        // Unsubscribe to prevent recursion
        teleportationToggle.onValueChanged.RemoveListener(OnTeleportationToggleChanged);
        smoothLocomotionToggle.onValueChanged.RemoveListener(OnSmoothLocomotionToggleChanged);

        // Set the toggle states and enable/disable the movement modes
        teleportationToggle.isOn = useTeleportation;
        smoothLocomotionToggle.isOn = !useTeleportation;

        playerTeleport.enabled = useTeleportation;
        smoothLocomotion.enabled = !useTeleportation;

        // Set interactability
        teleportationToggle.interactable = !useTeleportation;
        smoothLocomotionToggle.interactable = useTeleportation;

        // Re-subscribe to the OnValueChanged events
        teleportationToggle.onValueChanged.AddListener(OnTeleportationToggleChanged);
        smoothLocomotionToggle.onValueChanged.AddListener(OnSmoothLocomotionToggleChanged);
    }
}
