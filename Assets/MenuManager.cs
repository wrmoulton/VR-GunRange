using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnDropdownValueChanged(int value)
    {
        Debug.LogWarning($"the int is {value}");
        switch (value)
        {
            case 1:
                StartFreeForAll();
                break;
            case 2:
                StartSpeedTest();
                break;
            case 3:
                StartAccuracyTest();
                break;
            case 4: // New option for restarting the game
                RestartGame();
                break;
            default:
                Debug.LogWarning("Invalid dropdown value.");
                break;
        }
    }

    public void StartFreeForAll()
    {
        GameModeManager.Instance.currentGameMode = GameModeManager.GameMode.FreeForAll;
        LoadGunRange();
    }

    public void StartSpeedTest()
    {
        GameModeManager.Instance.currentGameMode = GameModeManager.GameMode.SpeedTest;
        LoadGunRange();
    }

    public void StartAccuracyTest()
    {
        GameModeManager.Instance.currentGameMode = GameModeManager.GameMode.AccuracyTest;
        LoadGunRange();
    }

    private void LoadGunRange()
    {
        SceneManager.LoadScene("Gun Range");
    }

    public void RestartGame()
    {
        // Reload the current scene while keeping the current game mode
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
