using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_LevelSpawner : MonoBehaviour
{
    //Private Variables
    private ARSessionOrigin _arOrigin;
    private Camera _arCamera;
    private ARRaycastManager _arRayManager;
    private bool _placementPoseIsValid;
    private Pose _placementPose;
    private bool _levelStartPlaced;
    private GameObject _placementIndicator;
    private GameObject _levelMap;
    private GameObject _player;

    void Awake()
    {
        GameManager.OnGameStateChanged += SpawnLevel;
    }

    void Start()
    {
        _arOrigin = FindObjectOfType<ARSessionOrigin>();
        _arCamera = _arOrigin.transform.GetChild(0).GetComponent<Camera>();
        _arRayManager = _arOrigin.GetComponent<ARRaycastManager>();
        _placementIndicator = GameObject.FindGameObjectWithTag("PlacementIndicator");
        _levelMap = Settings.Instance.levelMap;
        _player = Settings.Instance.player;

        if (_arOrigin == null || _arCamera == null || _arRayManager == null)
        {
            Debug.Log("One of the following components is missing: 'AR Session Origin', 'AR Camera', 'AR Raycast Manager'");
        }

        _placementIndicator.SetActive(false);
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    // - - - Function Archive - - - //

    private void UpdatePlacementPose()
    {
        //initiating a new variable to store the center of my phone screen
        var screenCenter = _arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //creating a empty list, in which I will store any hits by the raycast operation
        var hits = new List<ARRaycastHit>();

        //using the ARFoundation's inbuilt raycast operation to shoot a ray from the center of my screen into the "real world"
        _arRayManager.Raycast(screenCenter, hits, TrackableType.Planes);

        //sets placementPoseIsValid bool to true, if there is at least 1 hit in the raycast hit list.
        _placementPoseIsValid = hits.Count > 0;

        if (_placementPoseIsValid)
        {
            //The position of that hit is copied over to the placementPose object.
            _placementPose = hits[0].pose;

            //creating a variable, that acts as a forward vector of the camera direction.
            var cameraForward = _arCamera.transform.forward;

            //calculating a new rotation for the placement pose (and therefore the placement indicator), based on the camera direction. 
            //y orientation is determined wether the detected plane is vertical or horizontal.
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (_placementPoseIsValid && _levelStartPlaced == false)
        {
            _placementIndicator.SetActive(true);
            //the position and rotation of the placement indicator adapts the position and rotation of the placement pose.
            _placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else if ((!_placementPoseIsValid || _levelStartPlaced == true))
        {
            if (_placementIndicator.activeSelf == true)
            {
                _placementIndicator.SetActive(false);
            }
        }
    }

    public void InitializeGameSetup()
    {
        //Update the GameState
        GameManager.Instance.UpdateGameState(GameState.PlayAreaFound);
    }

    public void SpawnLevel(GameState state)
    {
        if (state == GameState.PlayAreaFound)
        {
#if UNITY_ANDROID
            if (_levelStartPlaced == false && _placementPoseIsValid == true)
            {
                //Spawning the level map
                Instantiate(_levelMap, _placementPose.position, _placementPose.rotation);

                //Spawning the player
                SpawnPlayer();

                _levelStartPlaced = true;

                //Update the GameState
                GameManager.Instance.UpdateGameState(GameState.PrepareEnemySpawning);
            }
#endif

#if UNITY_EDITOR
            if (_levelStartPlaced == false)
            {
                //Spawning the level map
                Instantiate(_levelMap, Vector3.zero, Quaternion.identity);

                //Spawning the player
                SpawnPlayer();

                _levelStartPlaced = true;

                //Update the GameState
                GameManager.Instance.UpdateGameState(GameState.PrepareEnemySpawning);
            }
#endif
        }
    }

    public void SpawnPlayer()
    {
        //Spawning the player
        Instantiate(_player, _placementPose.position + new Vector3(0.0f, 0.01f, 0.0f), _placementPose.rotation);
    }


    private void OnDisable()
    {
        //Unsubscribing functions from events
        GameManager.OnGameStateChanged -= SpawnLevel;
    }
}
