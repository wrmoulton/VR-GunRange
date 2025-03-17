using UnityEngine;
using TMPro;
using System.Collections;

public class ElPresidenteManager : MonoBehaviour
{
    public GameObject[] firstRoundTargets; // First set of targets
    public GameObject[] secondRoundTargets; // Second set of targets
    public TextMeshProUGUI timerText; // Timer display
    public TextMeshProUGUI countdownText; // Countdown display
    public GameObject targetParent; // Parent object containing all targets

    private float timer = 0f;
    private bool gameActive = false;
    private int targetsHit = 0;
    private int currentRound = 1;

    public void StartElPresidente()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);
        BeginElPresidente();
    }

    private void BeginElPresidente()
    {
        timer = 0f;
        targetsHit = 0;
        gameActive = true;

        // Activate first round of targets
        ActivateTargets(firstRoundTargets);

        Debug.Log("El Presidente started!");
    }

    private void Update()
    {
        if (gameActive)
        {
            timer += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = $"Time: {timer:F2} seconds";
            }
        }
    }

    public void TargetDestroyed()
    {
        targetsHit++;

        Debug.Log($"Target destroyed! {targetsHit}/{GetCurrentTargetSet().Length} targets destroyed.");

        if (targetsHit >= GetCurrentTargetSet().Length)
        {
            if (currentRound == 1)
            {
                ActivateSecondRound();
            }
            else
            {
                EndElPresidente();
            }
        }
    }

    private void ActivateSecondRound()
    {
        currentRound = 2;
        targetsHit = 0;

        Debug.Log("First round complete! Activating second round targets.");

        // Activate second round of targets
        ActivateTargets(secondRoundTargets);
    }

    private void EndElPresidente()
    {
        gameActive = false;
        Debug.Log($"El Presidente complete! Time: {timer:F2} seconds");

        if (timerText != null)
        {
            timerText.text = $"Completed in {timer:F2} seconds!";
        }

        // Deactivate all targets
        if (targetParent != null)
        {
            targetParent.SetActive(false);
        }
    }

    private void ActivateTargets(GameObject[] targets)
    {
        if (targets == null || targets.Length == 0) return;

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                target.SetActive(true);
            }
        }
    }

    private GameObject[] GetCurrentTargetSet()
    {
        return currentRound == 1 ? firstRoundTargets : secondRoundTargets;
    }
}
