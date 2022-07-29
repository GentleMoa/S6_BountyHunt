using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Targeting : MonoBehaviour
{
    //Private Variables
    private Vector3 _targetDirection;
    private Material _matEnemy;
    private Material _matEnemyInLOS;
    private Transform _ig11_raycastOrign;

    //Serialized Variables
    [SerializeField] private List<GameObject> enemiesInLOS = new List<GameObject>();

    void Start()
    {
        _matEnemy = Settings.Instance.matEnemy;
        _matEnemyInLOS = Settings.Instance.matEnemyInLOS;

        _ig11_raycastOrign = GameObject.FindGameObjectWithTag("IG11_RaycastOrigin").GetComponent<Transform>();
    }

    void Update()
    {
        CalculateTargetsInLineOfSight();
    }

    private void CalculateTargetsInLineOfSight()
    {
        foreach (GameObject enemy in EnemyPool.SharedInstance.pooledObjects)
        {
            //Get the respective enemy's transform
            var enemyTransform = enemy.GetComponent<Transform>();

            RaycastHit hit;
            _targetDirection = (enemy.transform.position - _ig11_raycastOrign.position).normalized;

            if (Physics.Raycast(_ig11_raycastOrign.position, _targetDirection, out hit))
            {
                if (hit.transform == enemyTransform)
                {
                    if (!enemiesInLOS.Contains(hit.transform.gameObject))
                    {
                        enemiesInLOS.Add(hit.transform.gameObject);
                        UpdateEnemiesMat(enemy.GetComponent<Renderer>(), _matEnemyInLOS);
                    }
                }
                else
                {
                    if (enemiesInLOS.Contains(enemy))
                    {
                        enemiesInLOS.Remove(enemy);
                        UpdateEnemiesMat(enemy.GetComponent<Renderer>(), _matEnemy);
                    }
                }
            }

            //For Debugging
            Debug.DrawRay(_ig11_raycastOrign.position, _targetDirection, Color.red);
        }
    }

    private void UpdateEnemiesMat(Renderer rnd, Material mat)
    {
        if (rnd.material != mat)
        {
            rnd.material = mat;
        }
    }
}
