using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private void Awake()
    {
        pool = new Dictionary<string, List<GameObject>>();
        prefabs = new Dictionary<string, GameObject>();

        foreach (GameObject gameObject in objectPrefabs)
        {
            pool[gameObject.name] = new List<GameObject>();
            prefabs[gameObject.name] = gameObject;
        }
    }

    [SerializeField]
    private GameObject[] objectPrefabs;

    private Dictionary<string, List<GameObject>> pool;
    private Dictionary<string, GameObject> prefabs;

    public GameObject GetObject(string type)
    {
        foreach (GameObject gameObject in pool[type])
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);

                return gameObject;
            }
        }

        GameObject newObject = Instantiate(prefabs[type]);
        pool[type].Add(newObject);
        newObject.name = type;

        return newObject;
    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
