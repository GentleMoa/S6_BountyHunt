using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.OnGameStateChanged += LoadScene_1;

        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {
        //Update the GameState
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    private void LoadScene_1(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            SceneManager.LoadScene("Scene_1");

            //Update the GameState
            GameManager.Instance.UpdateGameState(GameState.FindPlayArea);
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= LoadScene_1;
    }
}
