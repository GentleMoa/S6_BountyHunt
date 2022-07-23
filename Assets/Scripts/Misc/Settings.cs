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

    //For Script: AR_LevelSpawner
    [Header ("AR_LevelSpawner")]
    public GameObject levelMap;
    public GameObject player;


}
