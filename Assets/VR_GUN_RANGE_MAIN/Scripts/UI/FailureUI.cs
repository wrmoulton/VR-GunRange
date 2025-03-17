using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;

public class FailureUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro failureText; // UI element to display the timer

    public void UpdateFailureText(String displayStr)
    {
        string currentText = failureText.text;
        failureText.text = displayStr;
    }

    public void activateFailuresUI()
    {
        gameObject.SetActive(true);
    }

    public void deactivateFailureUI()
    {
        gameObject.SetActive(false);
    }
}
