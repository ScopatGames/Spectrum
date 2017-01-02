using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ItemController : NetworkBehaviour {
    [Header("Pool Prefabs by Deployment Method (increasing capability)")]

    public List<Pool> groundDefensesPoolsPrefabs = new List<Pool>();
    public List<Pool> neutralPickupsPoolsPrefabs = new List<Pool>();
    public List<Pool> bombDropsPoolsPrefabs = new List<Pool>();

    public List<Pool> groundDefensesPools = new List<Pool>();
    public List<Pool> neutralPickupsPools = new List<Pool>();
    public List<Pool> bombDropsPools = new List<Pool>();
    public List<PoolItem> deployedItems = new List<PoolItem>();

    //PUBLIC METHODS
    //--------------------------------------------------------
    public void DeployBombDrops(int level, int quantity)
    {
        Pool poolRef = bombDropsPools[level];
        CreatePoolItems(poolRef, quantity);
    }

    public void DeployGroundDefenses(int level, int quantity)
    {
        Pool poolRef = groundDefensesPools[level];
        CreatePoolItems(poolRef, quantity);

        GroundEvenDistribution(poolRef, quantity);
    }

    public void DeployNeutralPickups(int level, int quantity)
    {
        Pool poolRef = neutralPickupsPools[level];
        CreatePoolItems(poolRef, quantity);

        SpaceEvenDistribution(poolRef, quantity);
    }

    public void PoolSetup()
    {
        CreatePools(groundDefensesPoolsPrefabs, groundDefensesPools);
        CreatePools(neutralPickupsPoolsPrefabs, neutralPickupsPools);
        CreatePools(bombDropsPoolsPrefabs, bombDropsPools);
    }

    public void WithdrawDeployedItems()
    {
        if (deployedItems.Count > 0)
        {
            foreach (PoolItem poolItem in deployedItems)
            {
                poolItem.pool.CheckIn(poolItem);
            }
            deployedItems.Clear();
        }
    }

    //PRIVATE METHODS
    //--------------------------------------------------------

    private void CreatePools(List<Pool> poolPrefabs, List<Pool> pools)
    {
        foreach (Pool prefab in poolPrefabs)
        {
            GameObject newPool = (GameObject)Instantiate(prefab.gameObject);
            NetworkServer.Spawn(newPool);
            pools.Add(newPool.GetComponent<Pool>());
        }
    }

    public void CreatePoolItems(Pool pool, int quantity)
    {
        if (pool.CheckInventory() < quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                GameObject tempGameObject = (GameObject)Instantiate(pool.poolObjectPrefab.gameObject);
                tempGameObject.transform.parent = pool.transform;
                NetworkServer.Spawn(tempGameObject);
                PoolItem tempPoolItemRef = tempGameObject.GetComponent<PoolItem>();
                tempPoolItemRef.pool = pool;
                pool.CheckIn(tempPoolItemRef);
            }
        }
    }

    private void GroundEvenDistribution(Pool poolRef, int quantity)
    {
        float angleIncrement = Mathf.PI * 2 / quantity;
        for (int i = 0; i < quantity; i++)
        {
            if (poolRef.CheckInventory() > 0)
            {
                PoolItem temp = poolRef.CheckOut();
                deployedItems.Add(temp);
                float angleVariance = Random.Range(-0.1f, 0.1f);
                Vector2 origin = new Vector2(40f * Mathf.Cos(i * angleIncrement + angleVariance), 40f * Mathf.Sin(i * angleIncrement + angleVariance));
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero - origin, 60f, LayerMask.GetMask(_Layers.environment));
                temp.transform.position = hit.point;
                temp.transform.parent = hit.transform;
                temp.Initialize();
            }
        }
    }

    private void SpaceEvenDistribution(Pool poolRef, int quantity)
    {
        float angleIncrement = Mathf.PI * 2 / quantity;
        for (int i = 0; i < quantity; i++)
        {
            if (poolRef.CheckInventory() > 0)
            {
                PoolItem temp = poolRef.CheckOut();
                deployedItems.Add(temp);
                float angleVariance = Random.Range(-0.1f, 0.1f);
                Vector2 origin = new Vector2(22f * Mathf.Cos(i * angleIncrement + angleVariance), 22f * Mathf.Sin(i * angleIncrement + angleVariance));
                float distanceFactor = Random.Range(3f, 22f)/22f;
                temp.transform.position = distanceFactor*origin;
                temp.Initialize();
            }
        }
    }
}
