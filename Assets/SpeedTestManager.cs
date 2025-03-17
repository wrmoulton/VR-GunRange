using System.Collections;
using UnityEngine;

public class SpeedTestManager : MonoBehaviour
{
    public GameObject[] targets; // Assign all targets for the Speed Test
    [SerializeField] private CountdownUI CountDown_UI;
    [SerializeField] private TimerUI Timer_UI;
    public GameObject speedTestTargets; // Parent GameObject containing all targets
    public GameObject start;
    public GameObject description;
    public GameObject stand;

    private float timer = 0f;
    private bool gameActive = false;
    private int targetsHit = 0;

    public void StartSpeedTest()
    {
        description.SetActive(true);
        start.SetActive(true);
        stand.SetActive(true);
    }

    public void SpeedTest()
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

        // Start the actual Speed Test
        BeginSpeedTest();
    }

    private void BeginSpeedTest()
    {
        // Reset the timer and target hit count
        timer = 0f;
        targetsHit = 0;
        gameActive = true;

        // Activate the timer UI
        Timer_UI.ActivateTimerUI();
        Timer_UI.ResetTimer();
        Timer_UI.HideCompletionText();

        // Update the initial target status
        Timer_UI.UpdateTargetsStatus(targetsHit, targets.Length);

        // Activate all targets via their parent
        if (speedTestTargets != null)
        {
            speedTestTargets.SetActive(true);
        }

        Debug.Log("Speed Test started!");
    }

    private void Update()
    {
        // Update the timer while the game is active
        if (gameActive)
        {
            timer += Time.deltaTime;
            Timer_UI.UpdateTimer(timer);
        }
    }

    public void TargetDestroyed()
    {
        targetsHit++;

        // Update the targets status on the UI
        Timer_UI.UpdateTargetsStatus(targetsHit, targets.Length);

        Debug.Log($"Target destroyed! {targetsHit}/{targets.Length} targets destroyed.");

        if (targetsHit >= targets.Length)
        {
            EndSpeedTest();
        }
    }

    private void EndSpeedTest()
    {
        // Stop the timer
        gameActive = false;

        Debug.Log($"Speed Test complete! Time: {timer:F2} seconds");

        // Show completion time and final targets destroyed on the UI
        stand.SetActive(false);

        Timer_UI.UpdateTimer(timer);
        Timer_UI.ShowCompletionTime(timer);
    }
}
