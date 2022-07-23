using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Private Variables
    private GameObject _raycastOrigin;
    private AR_LevelSpawner ar_levelSpawner;
    private float _raycastOriginHeight;

    //Serialized Variables
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemySpawnRate = 1.5f;

    //Public Variables
    public List<GameObject> enemySpawnAreaCorners = new List<GameObject>();

    //Events and Delegates 
    public event Action D_enemySpawned;

    void Start()
    {
        //Finding the AR_LevelSpawner script by type (there is only one of those)
        ar_levelSpawner = GameObject.FindObjectOfType<AR_LevelSpawner>();

        if (ar_levelSpawner != null)
        {
            //Subscribing functions to the ar_levelSpawner.d_levelMapSpawned event
            ar_levelSpawner.D_levelMapSpawned += FindPlayAreaRelatedReferences;
            ar_levelSpawner.D_levelMapSpawned += RunPlayAreaRelatedLogic;
        }
        else
        {
            Debug.Log("The AR_LevelSpawner Script was not found!!");
        }
    }

    private void FindPlayAreaRelatedReferences()
    {
        //Finding the _raycastOrigin empty by tag
        _raycastOrigin = GameObject.FindGameObjectWithTag("RaycastOrigin");

        //Save the _raycastOrigin current y position
        _raycastOriginHeight = _raycastOrigin.transform.position.y;

        //Populate the enemySpawnAreaCorners List with the 4 Enemy_SpawnArea_Corners
        enemySpawnAreaCorners.AddRange(GameObject.FindGameObjectsWithTag("EnemySpawnArea_Corner"));
    }

    private void RunPlayAreaRelatedLogic()
    {
        //Calling the function to shoot a raycast and maybe spawn an enemy repeatately
        InvokeRepeating("DesignateEnemySpawnPos", 0.0f, enemySpawnRate);
    }

    private void DesignateEnemySpawnPos()
    {
        ReshuffleRaycastOrigin();

        RaycastHit hit;

        if (Physics.Raycast(_raycastOrigin.transform.position, -Vector3.up, out hit))
        {
            if (hit.transform.gameObject.tag == "EnemySpawnArea")
            {
                //For Debugging
                //Debug.Log("Raycast shot and hit EnemySpawnArea!");

                Instantiate(enemy, hit.point + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity);

                //Calling the D_enemySpawned delegate/event
                D_enemySpawned();
            }
            else
            {
                //For Debugging
                //Debug.Log("Raycast shot but did not hit EnemySpawnArea!");
            }
        }
    }

    private void ReshuffleRaycastOrigin()
    {
        //For Debugging
        //Debug.Log("Reshuffling RaycastOrigin!");

        _raycastOrigin.transform.position = new Vector3(UnityEngine.Random.Range(enemySpawnAreaCorners[0].transform.position.x, enemySpawnAreaCorners[1].transform.position.x), _raycastOriginHeight, UnityEngine.Random.Range(enemySpawnAreaCorners[0].transform.position.z, enemySpawnAreaCorners[3].transform.position.z));
    }

    private void OnDestroy()
    {
        //Unsubscribing functions from events
        ar_levelSpawner.D_levelMapSpawned -= FindPlayAreaRelatedReferences;
        ar_levelSpawner.D_levelMapSpawned -= RunPlayAreaRelatedLogic;
    }
}
