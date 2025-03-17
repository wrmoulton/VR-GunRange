using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] countdownTexts; // UI element to display the timer
    public void UpdateCountdown(String displayStr)
    {
        foreach (TextMeshPro roundSignal in countdownTexts)
        {
            string currentText = roundSignal.text;
            roundSignal.text = displayStr;
        }
    }

    public void ResetCountdown()
    {
        foreach (TextMeshPro roundSignal in countdownTexts)
        {
            string currentText = roundSignal.text;
            roundSignal.text = "5";
        }
    }

    public void activateCountdownUI()
    {
        gameObject.SetActive(true);
    }

    public void deactivateCountdownUI()
    {
        gameObject.SetActive(false);
    }
}
