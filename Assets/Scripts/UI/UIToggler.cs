using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BountyHunt.ExtensionMethods;

public class UIToggler : MonoBehaviour
{
    [SerializeField] GameObject[] uiToToggle;
    private AR_LevelSpawner _arLevelSpawner;

    // Start is called before the first frame update
    void Start()
    {
        _arLevelSpawner = FindObjectOfType<AR_LevelSpawner>();

        //Subscribing 'ToggleUI' to AR_LevelSpawner's delegate
        if (_arLevelSpawner != null)
        {
            _arLevelSpawner.d_levelMapSpawned += ToggleUI;
        }
    }

    private void ToggleUI()
    {
        uiToToggle.ToggleGameObjectArray(false);
    }

    private void OnDestroy()
    {
        _arLevelSpawner.d_levelMapSpawned -= ToggleUI;
    }
}

