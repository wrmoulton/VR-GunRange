using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using BNG;

[System.Serializable]
public class TargetData {
	public int targetNumber;
    public bool startOnLeft;
    public bool startUpright;
    public float delayStart;
    public float totalTime;
    public float movementDist1;
    public float stillTime;
}

[System.Serializable]
public class LevelData {
	public int numNeededPoints = 0;
	public List<TargetData> targets;
}

public class AccuracyTestManager : MonoBehaviour
{
    public List<LevelData> levels;
    public List<GameObject> targets;
    [SerializeField] private CountdownUI CountDown_UI;
    [SerializeField] private PointsTimerUI Timer_UI;
	public GameObject accuracyTestTargets; // Parent GameObject containing all targets
    public GameObject start;
    public GameObject description;
	public GameObject stand;
	public bool DEBUG = false;
	public int startingLevel = 0;

    private float timer = 0f;
    private bool gameActive = false;
    private int currentLevel = 0;
	private float curLevelEndTime = 0;
	private bool levelActive = false;

	private int numPoints = 0;

	private void Start() {
		if(DEBUG) {
			currentLevel = startingLevel;
			StartAccuracyTest();
		}
	}

	private void EnableAccuracyTestGuns() {
        // Find the child named "AccuracyTestGuns"
        Transform childTransform = transform.Find("AccuracyTestGuns");
        
        if (childTransform != null) {
            // Access the GameObject and enable it
            childTransform.gameObject.SetActive(true);
            // Debug.Log("AccuracyTestGuns has been enabled!");
        } else {
            Debug.LogWarning("Child named 'AccuracyTestGuns' not found!");
        }
    }

	public void StartAccuracyTest()
    {
        description.SetActive(true);
        start.SetActive(true);
        stand.SetActive(true);
		if(DEBUG) EnableAccuracyTestGuns();
    }

    public void AccuracyTest()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        CountDown_UI.activateCountdownUI();

        CountDown_UI.UpdateCountdown("5");
        yield return new WaitForSeconds(1f);

        CountDown_UI.UpdateCountdown("4");
        yield return new WaitForSeconds(1f);

        CountDown_UI.UpdateCountdown("3");
        yield return new WaitForSeconds(1f);

        CountDown_UI.UpdateCountdown("2");
        yield return new WaitForSeconds(1f);

        CountDown_UI.UpdateCountdown("1");
        yield return new WaitForSeconds(1f);

        CountDown_UI.UpdateCountdown("START");
        yield return new WaitForSeconds(1f);

        CountDown_UI.deactivateCountdownUI();
        description.SetActive(false);

        // Start the actual Accuracy Test
        BeginAccuracyTest();
    }

    private void BeginAccuracyTest()
    {
        if(levelActive) {
			Debug.LogWarning("There is a level currently active!!");
			return;
		}

        // Debug.Log($"Starting Level {currentLevel}");

        if (currentLevel >= levels.Count) {
            Debug.LogError("Invalid level index!");
            return;
        }
		
		levelActive = true;
		timer = 0f;
		numPoints = 0;
		gameActive = true;

        // Activate the timer UI
        Timer_UI.ActivateTimerUI();
        Timer_UI.ResetTimer();
        Timer_UI.HideCompletionText();

        // Update the initial target status
        Timer_UI.UpdateTargetsStatus(numPoints, levels[currentLevel].numNeededPoints);

        // Activate all targets via their parent
        if (accuracyTestTargets != null)
        {
            accuracyTestTargets.SetActive(true);
        }

		RespawnAllTargets();

		LevelData levelData = levels[currentLevel];

		float maxTime = 0;

        // Loop through targets and apply parameters
        for (int i = 0; i < levelData.targets.Count; i++) {
            int targetInd = levelData.targets[i].targetNumber;
			if (targetInd >= targets.Count) {
                Debug.LogWarning("Invalid target index!");
                break;
            }

            var target = targets[targetInd];
            var targetData = levelData.targets[i];
            var targetScript = target.GetComponent<MoveableTargets>();

            if (targetScript != null) {
                targetScript.TriggerMove(
                    targetData.startOnLeft,
                    targetData.startUpright,
                    targetData.delayStart,
                    targetData.totalTime,
                    targetData.movementDist1,
                    targetData.stillTime
                );
				if(targetData.totalTime > maxTime) maxTime = targetData.totalTime;
            } else {
                Debug.LogWarning($"Target at index {i} does not have a Target script.");
            }
        }

		curLevelEndTime = (float) Time.timeAsDouble + maxTime;

        // Debug.Log("Speed Test started!");
    }

	private void RespawnAllTargets() {
		foreach (GameObject obj in targets) {
            // Find the "Target" child
            Transform targetTransform = obj.transform.Find("Target");
            if (targetTransform != null) {
                // Iterate through all children of "Target"
                foreach (Transform child in targetTransform) {
                    // Check if the child has a Damageable component
                    Damageable damageable = child.GetComponent<Damageable>();
                    if (damageable != null) {
                        // Call the RespawnRoutine method
                        damageable.TriggerRespawn();
                    } else {
                        Debug.LogWarning($"Child {child.name} of 'Target' in GameObject {obj.name} does not have a Damageable script attached!");
                    }
                }
            } else {
                Debug.LogWarning($"GameObject {obj.name} does not have a child named 'Target'!");
            }
        }
	}

    private void Update()
    {
		// Update the timer while the game is active
        if (gameActive)
        {
            timer = curLevelEndTime - (float) Time.timeAsDouble;
			Timer_UI.UpdateTimer(timer);
			if(levelActive && timer <= 0 && curLevelEndTime != 0) {
				EndLevel();
				return;
			}
        }
    }

	public void AddPoints(int points) {
        numPoints += points;
		int numPointsNeeded = levels[currentLevel].numNeededPoints;
		Timer_UI.UpdateTargetsStatus(numPoints, numPointsNeeded);
    }

	private void EndLevel() {
		gameActive = false;
		levelActive = false;

		stand.SetActive(false);

		Timer_UI.ShowCompletion(numPoints, levels[currentLevel].numNeededPoints);
		if(numPoints >= levels[currentLevel].numNeededPoints) {
			currentLevel++;
		}
		AccuracyTest();
	}
}
