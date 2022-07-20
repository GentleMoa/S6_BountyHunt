using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    //Private Variables
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<GameObject> enemiesInLOS = new List<GameObject>();
    private Vector3 _targetDirection;
    private EnemySpawner _enemySpawner;
    private EnemySpawner_ForDebugging _enemySpawner_ForDebugging;

    void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _enemySpawner_ForDebugging = FindObjectOfType<EnemySpawner_ForDebugging>();

        if (_enemySpawner == null)
        {
            _enemySpawner_ForDebugging.D_enemySpawned += UpdateEnemiesList;
        }
        else if (_enemySpawner_ForDebugging == null)
        {
            _enemySpawner.D_enemySpawned += UpdateEnemiesList;
        }
    }


    void Update()
    {
        FindTargetsInLineOfSight();
    }

    private void FindTargetsInLineOfSight()
    {
        foreach (GameObject enemy in enemies)
        {
            RaycastHit hit;
            _targetDirection = (enemy.transform.position - gameObject.transform.position).normalized;

            if (Physics.Raycast(gameObject.transform.position, _targetDirection, out hit))
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    enemiesInLOS.Add(hit.transform.gameObject);

                    //For Debugging
                    Debug.Log("Enemy was hit by targeting raycast: " + hit.transform.gameObject.name);
                }
            }

            //For Debugging
            float _debugRayLength = 1.5f;
            Debug.DrawRay(gameObject.transform.position, _targetDirection * _debugRayLength, Color.red);
        }
    }

    private void UpdateEnemiesList()
    {
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //For Debugging
        Debug.Log("enemis count: " + enemies.Count + " enemiesInLOS count: " + enemiesInLOS.Count);
    }

    private void OnDestroy()
    {
        if (_enemySpawner == null)
        {
            _enemySpawner_ForDebugging.D_enemySpawned -= UpdateEnemiesList;
        }
        else if (_enemySpawner_ForDebugging == null)
        {
            _enemySpawner.D_enemySpawned -= UpdateEnemiesList;
        }
    }
}
