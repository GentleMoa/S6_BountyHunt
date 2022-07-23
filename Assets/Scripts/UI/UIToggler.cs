using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BountyHunt.ExtensionMethods;

public class UIToggler : MonoBehaviour
{
    //Private Variables
    private AR_LevelSpawner _arLevelSpawner;
    private AR_LevelSpawner_ForDebugging _ar_LevelSpawner_ForDebugging;

    //Serialized Variables
    [SerializeField] GameObject[] uiToToggle;

    // Start is called before the first frame update
    void Start()
    {
        _arLevelSpawner = FindObjectOfType<AR_LevelSpawner>();
        _ar_LevelSpawner_ForDebugging = FindObjectOfType<AR_LevelSpawner_ForDebugging>();

        if (_arLevelSpawner == null)
        {
            _ar_LevelSpawner_ForDebugging.D_levelMapSpawned += ToggleUI;
        }
        else if (_ar_LevelSpawner_ForDebugging == null)
        {
            _arLevelSpawner.D_levelMapSpawned += ToggleUI;
        }
    }

    private void ToggleUI()
    {
        uiToToggle.ToggleGameObjectArray(false);
    }

    private void OnDisable()
    {
        //if (_arLevelSpawner == null)
        //{
        //    _ar_LevelSpawner_ForDebugging.D_levelMapSpawned -= ToggleUI;
        //}
        //else if (_ar_LevelSpawner_ForDebugging == null)
        //{
        //    _arLevelSpawner.D_levelMapSpawned -= ToggleUI;
        //}
    }
}

