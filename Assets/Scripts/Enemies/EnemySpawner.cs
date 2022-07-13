using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Private Variables
    private GameObject _raycastOrigin;

    //Serialized Variables
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemySpawnRate = 1.5f;

    void Start()
    {
        _raycastOrigin = GameObject.FindGameObjectWithTag("RaycastOrigin");

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
                Debug.Log("Raycast shot and hit EnemySpawnArea!");

                Instantiate(enemy, hit.point + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity);
            }
            else
            {
                //For Debugging
                Debug.Log("Raycast shot but did not hit EnemySpawnArea!");
            }
        }
    }

    private void ReshuffleRaycastOrigin()
    {
        //For Debugging
        Debug.Log("Reshuffling RaycastOrigin!");

        _raycastOrigin.transform.position = new Vector3(Random.Range(-1.25f, 1.25f), 0.35f, Random.Range(-1.25f, 1.25f));
    }
}
