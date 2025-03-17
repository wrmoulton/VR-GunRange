using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using System.Globalization;

public class TactileStart : MonoBehaviour
{
    [SerializeField] private GameObject Podium;
    [SerializeField] private GameObject Initial_UI;
    [SerializeField] private SpeedTestManagerTactical TacticalManager;
    public static int NumberOfRounds = 10;
    public static float InitialSecondsPerRound = 45f;
    private static int BestRound = 1;

    public void GameStart()
    {
        if (Podium != null)
        {
            Podium.SetActive(false); // Disable the Podium
        }
        if (Initial_UI != null)
        {
            Initial_UI.SetActive(false); // Disable the Initial_UI
        }
        TacticalManager.StartRound();
    }
    public void GameEnd()
    {
        if (Podium != null)
        {
            Podium.SetActive(true); // Disable the Podium
        }
        if (Initial_UI != null)
        {
            Initial_UI.SetActive(true); // Disable the Initial_UI
        }

        //add respawn to inital breakable
    }
}
