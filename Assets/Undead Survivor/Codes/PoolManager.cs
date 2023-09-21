using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public GameObject[] prefabs;

    private List<GameObject>[] pools;

    private void Awake()
    {

        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0 ; i < prefabs.Length ; i++)
        {

            pools[i] = new List<GameObject>();

        }

    }

    public GameObject Get(int index)
    {

        GameObject obj = null;

        foreach(GameObject item in pools[index])
        {

            if (!item.activeSelf)
            {
                obj = item;
                obj.SetActive(true);
                break;
            }

        }

        if(!obj)
        {

            obj = Instantiate(prefabs[index], transform);
            pools[index].Add(obj);

        }

        return obj;

    }

}
