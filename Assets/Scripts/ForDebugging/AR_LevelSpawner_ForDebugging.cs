using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_LevelSpawner_ForDebugging : MonoBehaviour
{
    //Private Variables
    private ARSessionOrigin _arOrigin;
    private Camera _arCamera;
    private ARRaycastManager _arRayManager;
    private bool _placementPoseIsValid;
    private Pose _placementPose;
    private bool _levelStartPlaced;

    //Serialized Variables
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject levelMap;
    [SerializeField] private GameObject player;

    //Events and Delegates
    public event Action D_levelMapSpawned;

    void Start()
    {
        _arOrigin = FindObjectOfType<ARSessionOrigin>();
        _arCamera = _arOrigin.transform.GetChild(0).GetComponent<Camera>();
        _arRayManager = _arOrigin.GetComponent<ARRaycastManager>();

        if (_arOrigin == null || _arCamera == null || _arRayManager == null)
        {
            Debug.Log("One of the following components is missing: 'AR Session Origin', 'AR Camera', 'AR Raycast Manager'");
        }

        //placementIndicator.SetActive(false);

        //Subscribing 'SpawnPlayer' to the d_levelMapSpawned delegate
        D_levelMapSpawned += SpawnPlayer;
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
            placementIndicator.SetActive(true);
            //the position and rotation of the placement indicator adapts the position and rotation of the placement pose.
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        //else if ((!_placementPoseIsValid || _levelStartPlaced == true))
        //{
        //    placementIndicator.SetActive(false);
        //}
    }

    public void SpawnLevel()
    {
        if (_levelStartPlaced == false)
        {
            //Spawning the level map
            Instantiate(levelMap, _placementPose.position, _placementPose.rotation);

            _levelStartPlaced = true;

            if (D_levelMapSpawned != null)
            {
                D_levelMapSpawned();
            }
        }
    }

    public void SpawnPlayer()
    {
        //Spawning the player
        Instantiate(player, _placementPose.position + new Vector3(0.0f, 0.01f, 0.0f), _placementPose.rotation);
    }


    private void OnDestroy()
    {
        //Unsubscribing 'SpawnPlayer' to the d_levelMapSpawned delegate
        D_levelMapSpawned -= SpawnPlayer;
    }
}
