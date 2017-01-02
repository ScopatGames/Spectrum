using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Pool : NetworkBehaviour {

    public PoolItem poolObjectPrefab;

    List<PoolItem> pool = new List<PoolItem>();
    Vector3 poolPosition = new Vector3(1000f, 0f, 0f);

    public void CheckIn(PoolItem item)
    {
        item.Terminate();
        item.transform.position = poolPosition;
        item.transform.rotation = Quaternion.identity;
        pool.Add(item);
    }

    public PoolItem CheckOut()
    {
        if(pool.Count > 0)
        {
            PoolItem item = pool[0];
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
            GameObject tempGameObject = (GameObject)Instantiate(poolObjectPrefab.gameObject);
            tempGameObject.transform.parent = transform;
            PoolItem tempPoolItemRef = tempGameObject.GetComponent<PoolItem>();
            tempPoolItemRef.pool = this;
            CheckIn(tempPoolItemRef);
        }
    }

    [ClientRpc]
    public void RpcSpawnPoolObjects(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject tempGameObject = (GameObject)Instantiate(poolObjectPrefab.gameObject);
            tempGameObject.transform.parent = transform;
            NetworkServer.Spawn(tempGameObject);
            PoolItem tempPoolItemRef = tempGameObject.GetComponent<PoolItem>();
            tempPoolItemRef.pool = this;
            CheckIn(tempPoolItemRef);
        }
    }

    public void DestroyPoolObjects(int quantity)
    {
        for(int i = 0; i < quantity; i++)
        {
            PoolItem temp = pool[0];
            pool.RemoveAt(0);
            Destroy(temp.gameObject);
        }
    }
}
