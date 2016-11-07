using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

    public GameObject poolObjectPrefab;

    public List<GameObject> pool = new List<GameObject>();

    public void CheckIn(GameObject item)
    {
        item.transform.parent = transform;
        item.transform.position = transform.position;
        item.transform.rotation = Quaternion.identity;
        pool.Add(item);
    }

    public GameObject CheckOut()
    {
        if(pool.Count > 0)
        {
            GameObject item = pool[0];
            pool.RemoveAt(0);
            return item;
        }
        else
        {
            return null;
        }
    }

    public int CheckInventory()
    {
        return pool.Count;
    }

    public void InstantiatePoolObjects(int quantity)
    {
        for(int i=0; i < quantity; i++)
        {
            GameObject tempGameObject = (GameObject)Instantiate(poolObjectPrefab, transform.position, Quaternion.identity);
            tempGameObject.transform.parent = transform;
            pool.Add(tempGameObject);
        }
    }
}
