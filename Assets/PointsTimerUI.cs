using UnityEngine;
using TMPro;

public class PointsTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro timerText; // UI element to display the timer
    [SerializeField] private TextMeshPro targetsText; // UI element to display targets destroyed
    [SerializeField] private TextMeshPro completionText; // UI element to display completion time

    /// <summary>
    /// Updates the timer text with the current time.
    /// </summary>
    public void UpdateTimer(float time)
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {time:F2} seconds";
        }
    }

    /// <summary>
    /// Resets the timer display to its initial state.
    /// </summary>
    public void ResetTimer()
    {
        if (timerText != null)
        {
            timerText.text = "Time: 0.00 seconds";
        }
    }

    /// <summary>
    /// Updates the targets destroyed status.
    /// </summary>
    public void UpdateTargetsStatus(int curNumPoints, int numPointsNeeded)
    {
        if (targetsText != null)
        {
            targetsText.text = $"Points: {curNumPoints} / {numPointsNeeded}";
        }
    }

    /// <summary>
    /// Displays the completion time when the game ends.
    /// </summary>
    public void ShowCompletion(int curNumPoints, int numPointsNeeded)
    {
        if (completionText != null)
        {
            if(curNumPoints >= numPointsNeeded) {
				completionText.text = $"Accuracy Test Complete!";
			} else {
				int numPointsLeft = numPointsNeeded - curNumPoints;
				completionText.text = $"Accuracy Test Failed. Needed {numPointsLeft} more points.";
			}
            targetsText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            completionText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the completion text.
    /// </summary>
    public void HideCompletionText()
    {
        if (completionText != null)
        {
            completionText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates the timer UI.
    /// </summary>
    public void ActivateTimerUI()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates the timer UI.
    /// </summary>
    public void DeactivateTimerUI()
    {
        gameObject.SetActive(false);
    }
}
