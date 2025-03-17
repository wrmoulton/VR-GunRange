using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;

public class EndlessUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro endlessText; // UI element to display the timer

    [SerializeField] private Button endlessButton; 
    private bool endlessEnabledAlready = false;
    public void UpdateEndlessText(String displayStr)
    {
        string currentText = endlessText.text;
        endlessText.text = displayStr;
    }

    public void enableEndlessMode()
    {
        endlessEnabledAlready = true;
        endlessButton.interactable = false;
    }
    public void activateEndlessUI()
    {
        gameObject.SetActive(true);
    }

    public void deactivateEndlessUI()
    {
        gameObject.SetActive(false);
    }
}
