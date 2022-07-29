using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Private Variables
    private GameObject _raycastOrigin;
    private float _raycastOriginHeight;
    private float _enemySpawnRate = 1.5f;
    private List<GameObject> enemySpawnAreaCorners = new List<GameObject>();

    void OnEnable()
    {
        GameManager.OnGameStateChanged += FindPlayAreaRelatedReferences;
        GameManager.OnGameStateChanged += RunPlayAreaRelatedLogic;
    }

    private void FindPlayAreaRelatedReferences(GameState state)
    {
        if (state == GameState.PrepareEnemySpawning)
        {
            //Finding the _raycastOrigin empty by tag
            _raycastOrigin = GameObject.FindGameObjectWithTag("RaycastOrigin");

            //Save the _raycastOrigin current y position
            _raycastOriginHeight = _raycastOrigin.transform.position.y;

            //Populate the enemySpawnAreaCorners List with the 4 Enemy_SpawnArea_Corners
            enemySpawnAreaCorners.AddRange(GameObject.FindGameObjectsWithTag("EnemySpawnArea_Corner"));

            //Update the GameState
            GameManager.Instance.UpdateGameState(GameState.BeginEnemySpawning);
        }
    }

    private void RunPlayAreaRelatedLogic(GameState state)
    {
        if (state == GameState.BeginEnemySpawning)
        {
            //Calling the function to shoot a raycast and maybe spawn an enemy repeatately
            InvokeRepeating("DesignateEnemySpawnPos", 0.0f, _enemySpawnRate);

            //Update the GameState
            GameManager.Instance.UpdateGameState(GameState.Gameplay);
        }
    }

    private void DesignateEnemySpawnPos()
    {
        ReshuffleRaycastOrigin();

        RaycastHit hit;

        if (Physics.Raycast(_raycastOrigin.transform.position, -Vector3.up, out hit))
        {
            if (hit.transform.gameObject.tag == "EnemySpawnArea")
            {
                //Activating an enemy from the object pool
                GameObject _enemy = EnemyPool.SharedInstance.GetPooledObject();
                if (_enemy != null)
                {
                    _enemy.transform.position = hit.point + new Vector3(0.0f, 0.1f, 0.0f);
                    _enemy.transform.rotation = Quaternion.identity;
                    _enemy.SetActive(true);
                }
            }
        }
    }

    private void ReshuffleRaycastOrigin()
    {
        _raycastOrigin.transform.position = new Vector3(UnityEngine.Random.Range(enemySpawnAreaCorners[0].transform.position.x, enemySpawnAreaCorners[1].transform.position.x), _raycastOriginHeight, UnityEngine.Random.Range(enemySpawnAreaCorners[0].transform.position.z, enemySpawnAreaCorners[3].transform.position.z));
    }

    private void OnDisable()
    {
        //Unsubscribing functions from events
        GameManager.OnGameStateChanged -= FindPlayAreaRelatedReferences;
        GameManager.OnGameStateChanged -= RunPlayAreaRelatedLogic;
    }
}
