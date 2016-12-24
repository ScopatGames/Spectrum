using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

    public GameObject poolObjectPrefab;

    List<GameObject> pool = new List<GameObject>();
    Vector3 poolPosition = new Vector3(1000f, 0f, 0f);
    int count = 0;

    public void CheckIn(GameObject item)
    {
        item.transform.position = poolPosition;
        item.transform.rotation = Quaternion.identity;
        pool.Add(item);
        count++;
    }

    public GameObject CheckOut()
    {
        if(pool.Count > 0)
        {
            GameObject item = pool[0];
            pool.RemoveAt(0);
            count--;
            return item;
        }
        else
        {
            return null;
        }
    }

    public int CheckInventory()
    {
        return count;
    }

    public void InstantiatePoolObjects(int quantity)
    {
        for(int i=0; i < quantity; i++)
        {
            GameObject tempGameObject = (GameObject)Instantiate(poolObjectPrefab, poolPosition, Quaternion.identity);
            tempGameObject.GetComponent<PoolItem>().pool = this;
            tempGameObject.transform.parent = transform;
            pool.Add(tempGameObject);
            count++;
        }
    }

    public void DestroyPoolObjects(int quantity)
    {
        for(int i = 0; i < quantity; i++)
        {
            GameObject temp = pool[0];
            pool.RemoveAt(0);
            Destroy(temp);
        }
    }
}
