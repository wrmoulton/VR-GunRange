using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import Unity UI namespace for Text components
using TMPro;
using System;

public class RoundUIManager : MonoBehaviour
{   
    [SerializeField] private TextMeshPro[] roundTexts; // UI element to display the timer
    public void UpdateRound(int currentRound)
    {
        foreach (TextMeshPro roundSignal in roundTexts)
        {
            string currentText = roundSignal.text;
            roundSignal.text = "Round " + currentRound;
        }
    }

    public void ResetRounds()
    {
        foreach (TextMeshPro roundSignal in roundTexts)
        {
            string currentText = roundSignal.text;
            roundSignal.text = "Round 1";
        }
    }

    public void activateRoundUI()
    {
        gameObject.SetActive(true);
    }

    public void deactivateRoundUI()
    {
        gameObject.SetActive(false);
    }
}
