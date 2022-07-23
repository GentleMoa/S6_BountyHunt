using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    //Private Variables
    
    private Vector3 _targetDirection;
    private EnemySpawner _enemySpawner;
    private GameObject _lastEnemyHit;
    private Material _matEnemy;
    private Material _matEnemyInLOS;
    private Transform _ig11_raycastOrign;

    //Serialized Variables
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<GameObject> enemiesInLOS = new List<GameObject>();

    void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _ig11_raycastOrign = GameObject.FindGameObjectWithTag("IG11_RaycastOrigin").GetComponent<Transform>();

        //The D_enemySpawned event is triggered each time an enemy is spawned, every time that happens we want to update the enemies list
        if (_enemySpawner != null)
        {
            _enemySpawner.D_enemySpawned += UpdateEnemiesList;
        }
    }

    void Update()
    {
        CalculateTargetsInLineOfSight();
        //UpdateEnemiesMat();
        //UpdateEnemiesInLOSMat();
    }

    private void CalculateTargetsInLineOfSight()
    {
        foreach (GameObject enemy in enemies)
        {
            RaycastHit hit;
            _targetDirection = (enemy.transform.position - _ig11_raycastOrign.position).normalized;

            if (Physics.Raycast(_ig11_raycastOrign.position, _targetDirection, out hit))
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
            Debug.DrawRay(_ig11_raycastOrign.position, _targetDirection, Color.red);
            //Debug.Log("Raycast hit: " + hit.transform.gameObject.name);
        }

        foreach (GameObject enemyInLOS in enemiesInLOS)
        {
            RaycastHit hit;
            _targetDirection = (enemyInLOS.transform.position - _ig11_raycastOrign.position).normalized;

            if (Physics.Raycast(_ig11_raycastOrign.position, _targetDirection, out hit))
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

            if (_renderer.material != _matEnemy)
            {
                _renderer.material = _matEnemy;
            }
        }
    }
    private void UpdateEnemiesInLOSMat()
    {
        foreach (GameObject enemyInLOS in enemiesInLOS)
        {
            Renderer _renderer;
            _renderer = enemyInLOS.GetComponent<Renderer>();

            if (_renderer.material != _matEnemyInLOS)
            {
                _renderer.material = _matEnemyInLOS;
            }
        }
    }

    private void OnDisable()
    {
        _enemySpawner.D_enemySpawned -= UpdateEnemiesList;
    }
}
