using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class SpeedTestManagerTactical : MonoBehaviour
{
    [SerializeField] private RoundUIManager Round_Start_UI;
    [SerializeField] private CountdownUI CountDown_UI;
    [SerializeField] private EndlessUI EndlessUI;

    [SerializeField] private FailureUI FailureUI;


    [SerializeField] private GameObject TimerUI;
    [SerializeField] private TextMeshPro timerText; // UI element to display the timer
    [SerializeField] private TextMeshPro targetText; // UI element to display the timer

    public TacticalTargetManager BreakableTargetParent; // Parent GameObject containing all targets

    private float timer = 0f;
    private float initalTimeLimit = TactileStart.InitialSecondsPerRound;
    private float maxNumberOfRounds = TactileStart.NumberOfRounds;

    private bool gameActive = false;

    private bool uiActive = false;

    private bool endlessEnabled = false;

    private int quota = 10;
    private int currQuota = 0;

    private int currentRound = 0;


    public void StartRound()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Show countdown numbers (3, 2, 1, GO!)
        CountDown_UI.activateCountdownUI();
        if(!uiActive)
        {
            Round_Start_UI.activateRoundUI();
            TimerUI.SetActive(true);
            uiActive = true;
        }

        Debug.Log("Player Countdown");
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

        ProgressRound();
    }

    private void ProgressRound()
    {
        if(currentRound < maxNumberOfRounds || endlessEnabled)
        {
            currQuota = 0;
            quota = 10 + currentRound*2;
            currentRound++;
            Round_Start_UI.UpdateRound(currentRound);
            // Reset the timer
            timer = initalTimeLimit;
            timerText.text = $"Time Left: {timer:F2} seconds";
            if(currentRound != 1)
            {
                initalTimeLimit = initalTimeLimit - 1f;
            }
            // Activate all targets via their parent
            if (BreakableTargetParent != null)
            {
                if (quota > BreakableTargetParent.getBreakablesCount())
                {
                    quota = BreakableTargetParent.getBreakablesCount();
                }
                BreakableTargetParent.ActivateRoundTargets(quota);
            }
            targetText.text = $"Targets Left: {currQuota}/{quota}";
            gameActive = true;
            Debug.Log("Speed Test started!");
        }
        
    }

    public void incrementQuota ()
    {
        if(gameActive)
        {
            currQuota++;
            targetText.text = $"Targets Left: {currQuota}/{quota}";
        }
    }

    private void Update()
    {
        // Update the timer while the game is active
        if (gameActive)
        {
            timer -= Time.deltaTime;
            if(timerText!= null)
            {
                timerText.text = $"Time Left: {timer:F2} seconds";
            }
            if(timer <= 0 || currQuota == quota)
            {
                if(currQuota == quota)
                {
                    //progress a round
                    Debug.Log("Progressing round");
                    gameActive = false;
                    timerText.text = $"Round Completed in {timer:F2}";
                    if(currentRound < maxNumberOfRounds ||  endlessEnabled)
                    {
                        StartRound();
                    }
                    else
                    {
                        EndlessUI.activateEndlessUI();
                    }
                }
                else
                {
                    EndTacticalAssault();
                }
            }
        }
    }

    private void EndTacticalAssault()
    {
        // Stop the timer and display the final time
        gameActive = false;

        if (timerText != null)
        {
            timerText.text = $"Round Failed!";

        }
        if (endlessEnabled)
        {
            EndlessUI.activateEndlessUI();
            EndlessUI.UpdateEndlessText($"In your endless run of Tactical Assault, you reached round {currentRound} rounds. Feel free to try again by restarting the world!");
        }
        else
        {
            FailureUI.activateFailuresUI();
            FailureUI.UpdateFailureText($"In your run of Tactical Assault, you reached round {currentRound} /{maxNumberOfRounds}");
        }

    }
    public void EndlessTacticalAssault()
    {
        // Stop the timer and display the final time
        endlessEnabled = true;
        EndlessUI.enableEndlessMode();
        EndlessUI.UpdateEndlessText($"Congratulations! You have beaten the base game of Tactical Assault with {maxNumberOfRounds}/{maxNumberOfRounds} rounds. You have proven youself as a proficient shooter!");
        gameActive = true;
        EndlessUI.deactivateEndlessUI();
    }

}
