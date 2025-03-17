using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFX_Manager : MonoBehaviour
{
    public static SFX_Manager Instance;

    [Range(0f, 1f)]
    public float masterVolume = 0.75f; // Master volume control (0 - 1)

    private float oldMasterVolume = 0f; // Stores the previous master volume

    public AudioSource audioSource;
    public AudioClip demoSound;

    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();

    [Header("UI Elements")]
    public Slider volumeSlider; // Slider component to control master volume

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        RegisterAllAudioSources(); // Register all audio sources in the scene
    }

    private void RegisterAllAudioSources()
    {
        // Find all AudioSources in the scene
        AudioSource[] sources = FindObjectsOfType<AudioSource>();

        // Play a demo sound (optional)
        if (audioSource != null && demoSound != null)
        {
            audioSource.PlayOneShot(demoSound);
        }

        foreach (AudioSource source in sources)
        {
            // If the AudioSource is not already registered, store its original volume
            if (!originalVolumes.ContainsKey(source))
            {
                originalVolumes[source] = source.volume;
            }

            // Adjust the volume based on the masterVolume and the original volume
            if (oldMasterVolume > 0)
            {
                source.volume = Mathf.Clamp01((originalVolumes[source] / oldMasterVolume) * masterVolume);
            }
            else
            {
                source.volume = Mathf.Clamp01(originalVolumes[source] * masterVolume);
            }
        }

        // Update the oldMasterVolume for future adjustments
        oldMasterVolume = masterVolume;
    }

    public void SetMasterVolume()
    {
        if (volumeSlider == null) return;

        // Get the new master volume from the slider
        masterVolume = Mathf.Clamp01(volumeSlider.value);

        // Update all audio sources with the new volume
        RegisterAllAudioSources();
    }
}
