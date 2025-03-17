using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public enum GameMode { FreeForAll, SpeedTest, AccuracyTest }
    public GameMode currentGameMode = GameMode.FreeForAll; // Default to FreeForAll

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
