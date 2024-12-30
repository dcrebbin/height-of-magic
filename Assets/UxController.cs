using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UxController : MonoBehaviour
{

    public GameObject introScreen;
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    public void ShowIntroScreen()
    {
        introScreen.SetActive(true);
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    public void ShowDefeatScreen()
    {
        defeatScreen.SetActive(true);
    }
}
