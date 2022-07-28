using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    //Public Variables
    public static EnemyPool SharedInstance;
    public List<GameObject> pooledObjects;

    //Serialized Variables
    [SerializeField] int poolSize;

    //Private Variables
    private GameObject _enemy;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        _enemy = Settings.Instance.enemy;

        pooledObjects = new List<GameObject>();
        GameObject _go;
        for (int i = 0; i < poolSize; i++)
        {
            _go = Instantiate(_enemy);
            _go.SetActive(false);
            pooledObjects.Add(_go);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
