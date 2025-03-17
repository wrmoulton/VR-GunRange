using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using BNG;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject playerController;

    [Header("Scene Settings")]
    public string targetScene; // The name of the scene to load

    [Header("Fade Settings")]
    public float fadeDuration = 10f; // Duration of the fade effect

    [Header("Sound Settings")]
    public AudioSource audioSource; // Reference to an AudioSource for the transition sound
    public AudioClip transitionSound; // The transition sound effect

    private bool isTransitioning = false; // Prevents multiple transitions at once

    public bool nonColliderTransition = false; 

    public bool FadeScreenOnSwap = true;

    CharacterController controller;
    BNGPlayerController playerRig;
    CapsuleCollider playerCollider;
    Transform cameraRig;
    ScreenFader fader;

    private void Start()
    {
        InitializePlayerComponents();
    }

    private void InitializePlayerComponents()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned.");
            return;
        }

        playerRig = playerController.GetComponent<BNGPlayerController>();
        playerCollider = playerController.GetComponent<CapsuleCollider>();
        cameraRig = playerRig.CameraRig;
        fader = cameraRig.GetComponentInChildren<ScreenFader>();
    }

    private IEnumerator SwapSceneCoroutine()
    {
        isTransitioning = true;
        InitializePlayerComponents();
        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        if (FadeScreenOnSwap && fader)
        {
            fader.FadeInSpeed = fadeDuration;
            fader.DoFadeIn();
            yield return new WaitForSeconds(fadeDuration);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Reinitialize player-related components in the new scene
        InitializePlayerAfterSceneLoad();

        if (FadeScreenOnSwap && fader)
        {
            fader.FadeOutSpeed = fadeDuration;
            fader.DoFadeOut();
        }

        isTransitioning = false;
    }

    public void SceneSwap()
    {
        Debug.Log("Player Attempting Swap");
        StartCoroutine(SwapSceneCoroutine());
    }
    private void InitializePlayerAfterSceneLoad()
    {
        // Find the PlayerController object in the new scene
        playerController = GameObject.Find("PlayerController");

        // Check if the PlayerController object exists
        if (playerController == null)
        {
            Debug.LogError("PlayerController object not found in the new scene!");
            return;
        }

        // Check if the PlayerController object has the "Player" tag
        if (!playerController.CompareTag("Player"))
        {
            Debug.LogError("Found PlayerController, but it does not have the 'Player' tag!");
            playerController = null; // Reset to null to prevent unintended behavior
            return;
        }

        // Initialize player components
        InitializePlayerComponents();
    }
}
