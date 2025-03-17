using UnityEngine;

public class GunRangeManager : MonoBehaviour
{
    public GameObject freeForAllTargets; 
    public GameObject speedTestTargets;
    public GameObject accuracyTestTargets; 
    public SpeedTestManager speedTestManager;
    public AccuracyTestManager accuracyTestManager; 
    public GameObject freeForAllGuns;
    public GameObject speedTestGuns;

    private void Start()
    {
        // Check the current game mode from GameModeManager
        if (GameModeManager.Instance != null)
        {
            switch (GameModeManager.Instance.currentGameMode)
            {
                case GameModeManager.GameMode.FreeForAll:
                    SetupFreeForAll();
                    break;

                case GameModeManager.GameMode.SpeedTest:
                    SetupSpeedTest();
                    break;
                
                case GameModeManager.GameMode.AccuracyTest:
                    SetupAccuracyTest();
                    break;

                default:
                    Debug.LogWarning("Unknown game mode! Defaulting to Free For All.");
                    SetupFreeForAll();
                    break;
            }
        }
        else
        {
            Debug.LogWarning("GameModeManager instance is null. Defaulting to Free For All.");
            SetupFreeForAll();
        }
    }

    private void SetupFreeForAll()
    {
        // Activate Free For All targets and deactivate Speed Test targets/guns
        if (freeForAllTargets != null) freeForAllTargets.SetActive(true);
        if (speedTestTargets != null) speedTestTargets.SetActive(false);
        if(speedTestGuns != null) speedTestGuns.SetActive(false);
        if(freeForAllGuns != null) freeForAllGuns.SetActive(true);


        Debug.Log("Free For All mode setup completed.");
    }

    private void SetupSpeedTest()
    {
        // Deactivate Free For All targets and activate Speed Test targets/guns
        if (freeForAllTargets != null) freeForAllTargets.SetActive(false);
        if (speedTestTargets != null) speedTestTargets.SetActive(false);
        if(speedTestGuns != null) speedTestGuns.SetActive(true);
        if(freeForAllGuns != null) freeForAllGuns.SetActive(false);
        // Start the Speed Test logic
        if (speedTestManager != null)
        {
            speedTestManager.StartSpeedTest();
        }

        Debug.Log("Speed Test mode setup completed.");
    }

    private void SetupAccuracyTest()
    {
        //deactiavte
        if (freeForAllTargets != null) freeForAllTargets.SetActive(false);
        if (accuracyTestTargets != null) accuracyTestTargets.SetActive(false);
        if(speedTestGuns != null) speedTestGuns.SetActive(false);
        if(freeForAllGuns != null) freeForAllGuns.SetActive(false);
        // Start the El Presidente logic
        if (accuracyTestManager != null)
        {
            accuracyTestManager.StartAccuracyTest();
        }

        Debug.Log("Speed Test mode setup completed.");
    }
}
