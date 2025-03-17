using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour {
    public static PointsManager instance;
    private int totalPoints = 0;

    public Text pointsText;

	// Using awake to ensure that the manager is initialized first
    private void Awake() {
        if (instance == null) {
			instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points) {
        totalPoints += points;
        UpdatePointsDisplay();
    }

	// Resets totalPoints to 0
	public void resetPoints() {
		totalPoints = 0;
		UpdatePointsDisplay();
	}

    private void UpdatePointsDisplay() {
		if (pointsText != null) {
			pointsText.text = "Points: " + totalPoints.ToString();
		} else {
		}
	}
}