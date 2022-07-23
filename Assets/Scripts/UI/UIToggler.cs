using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BountyHunt.ExtensionMethods;

public class UIToggler : MonoBehaviour
{
    //Serialized Variables
    [SerializeField] GameObject[] uiToToggle_Button_SpawnLevel;

    void Awake()
    {
        GameManager.OnGameStateChanged += ToggleUI;
    }

    private void ToggleUI(GameState state)
    {
        if (state == GameState.PrepareEnemySpawning)
        {
            uiToToggle_Button_SpawnLevel.ToggleGameObjectArray(false);
        }
    }

    private void OnDisable()
    {
        //Unsubscribing functions from events
        GameManager.OnGameStateChanged -= ToggleUI;
    }
}

