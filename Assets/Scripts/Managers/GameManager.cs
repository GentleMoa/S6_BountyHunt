using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    //Singleton pattern
    public static GameManager Instance
    {
        get
        {
            return FindObjectOfType<GameManager>();
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        UpdateGameState(GameState.Beginning);
    }

    public void UpdateGameState(GameState newState)
    {
        Debug.Log("Old State: " + State);

        State = newState;

        Debug.Log("New State: " + State);

        switch (newState)
        {
            case GameState.Beginning:
                break;
            case GameState.MainMenu:
                break;
            case GameState.FindPlayArea:
                break;
            case GameState.PlayAreaFound:
                break;
            case GameState.PrepareEnemySpawning:
                break;
            case GameState.BeginEnemySpawning:
                break;
            case GameState.Gameplay:
                break;
            case GameState.Ending:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

//GameStates
public enum GameState
{
    Beginning,
    MainMenu,
    FindPlayArea,
    PlayAreaFound,
    PrepareEnemySpawning,
    BeginEnemySpawning,
    Gameplay,
    Ending
}
