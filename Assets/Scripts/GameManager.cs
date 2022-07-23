using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager Instance
    {
        get
        {
            return FindObjectOfType<GameManager>();
        }
    }

    //Event Handlers
}
