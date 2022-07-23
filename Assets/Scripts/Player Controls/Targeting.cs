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
    private GameObject _lastEnemyHit;

    //Serialized Variables
    [SerializeField] private Transform ig11_raycastOrign;
    [SerializeField] private Material matEnemy;
    [SerializeField] private Material matEnemyInLOS;

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
        CalculateTargetsInLineOfSight();
        UpdateEnemiesMat();
        UpdateEnemiesInLOSMat();
    }

    private void CalculateTargetsInLineOfSight()
    {
        foreach (GameObject enemy in enemies)
        {
            RaycastHit hit;
            _targetDirection = (enemy.transform.position - ig11_raycastOrign.position).normalized;

            if (Physics.Raycast(ig11_raycastOrign.position, _targetDirection, out hit))
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    if (!enemiesInLOS.Contains(hit.transform.gameObject))
                    {
                        enemiesInLOS.Add(hit.transform.gameObject);
                    }
                }

                //For Debugging
                //Debug.Log("Enemy was hit by targeting raycast: " + hit.transform.gameObject.name);
            }

            //For Debugging
            Debug.DrawRay(ig11_raycastOrign.position, _targetDirection, Color.red);
            //Debug.Log("Raycast hit: " + hit.transform.gameObject.name);
        }

        foreach (GameObject enemyInLOS in enemiesInLOS)
        {
            RaycastHit hit;
            _targetDirection = (enemyInLOS.transform.position - ig11_raycastOrign.position).normalized;

            if (Physics.Raycast(ig11_raycastOrign.position, _targetDirection, out hit))
            {
                Debug.Log("Raycast hit: " + hit.transform.gameObject.name);

                if (hit.transform.gameObject.tag == "Enemy")
                {
                    _lastEnemyHit = hit.transform.gameObject;
                }
                else if (hit.transform.gameObject.tag != "Enemy")
                {
                    if (enemiesInLOS.Contains(_lastEnemyHit))
                    {
                        enemiesInLOS.Remove(_lastEnemyHit);
                    }
                }
            }
        }
    }

    private void UpdateEnemiesList()
    {
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    private void UpdateEnemiesMat()
    {
        foreach (GameObject enemy in enemies)
        {
            Renderer _renderer;
            _renderer = enemy.GetComponent<Renderer>();

            if (_renderer.material != matEnemy)
            {
                _renderer.material = matEnemy;
            }
        }
    }
    private void UpdateEnemiesInLOSMat()
    {
        foreach (GameObject enemyInLOS in enemiesInLOS)
        {
            Renderer _renderer;
            _renderer = enemyInLOS.GetComponent<Renderer>();

            if (_renderer.material != matEnemyInLOS)
            {
                _renderer.material = matEnemyInLOS;
            }
        }
    }

    private void OnDisable()
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
