using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    //Singleton pattern for easy access
    public static Settings Instance
    {
        get
        {
            return FindObjectOfType<Settings>();
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //Reference Archive

    //For Script: AR_LevelSpawner --> On "GameLogicHolder" GameObject, in "Scene_1"
    [Header ("AR_LevelSpawner")]
    public GameObject levelMap;
    public GameObject player;

    //For Script: EnemySpawner --> On "GameLogicHolder" GameObject, in "Scene_1"
    [Header("EnemySpawner")]
    public GameObject enemy;

    //For Script: Targeting --> On "IG11_Player" Prefab, spawned in "Scene_1"
    [Header ("Targeting")]
    public Material matEnemy;
    public Material matEnemyInLOS;

}
