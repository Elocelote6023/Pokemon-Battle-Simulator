using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject battleSys;

    public event Action WildEncounter;
    public void PlayGame()
    {
        WildEncounter();
    }
}
