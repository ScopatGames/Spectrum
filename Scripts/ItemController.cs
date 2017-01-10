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

    private const float ANGLE_VARIANCE = 0.1f;
    private const float INNER_RADIUS = 3f;
    private const float OUTER_RADIUS = 22f;

    //PUBLIC METHODS
    //--------------------------------------------------------
    public void PrepareBombDrops(int level, int quantity)
    {
        Pool poolRef = bombDropsPools[level];
        CreatePoolItems(poolRef, quantity);
    }

    public void DeployGroundDefenses(int level, int quantity)
    {
        Pool poolRef = groundDefensesPools[level];
        CreatePoolItems(poolRef, quantity);
        float angleIncrement = Mathf.PI * 2 / quantity;
        for (int i = 0; i < quantity; i++)
        {
            if (poolRef.CheckInventory() > 0)
            {
                PoolItem temp = poolRef.CheckOut();
                deployedItems.Add(temp);
                float angleVariance = Random.Range(-ANGLE_VARIANCE, ANGLE_VARIANCE);
                Vector2 origin = new Vector2(OUTER_RADIUS * Mathf.Cos(i * angleIncrement + angleVariance), OUTER_RADIUS * Mathf.Sin(i * angleIncrement + angleVariance));
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero - origin, OUTER_RADIUS, LayerMask.GetMask(_Layers.environment));
                temp.transform.position = hit.point;
                temp.transform.parent = hit.transform;
                temp.RpcInitialize();
            }
        }
    }

    public void DeployNeutralPickups(int level, int quantity)
    {
        Pool poolRef = neutralPickupsPools[level];
        CreatePoolItems(poolRef, quantity);
        float angleIncrement = Mathf.PI * 2 / quantity;
        for (int i = 0; i < quantity; i++)
        {
            if (poolRef.CheckInventory() > 0)
            {
                PoolItem temp = poolRef.CheckOut();
                deployedItems.Add(temp);
                float angleVariance = Random.Range(-ANGLE_VARIANCE, ANGLE_VARIANCE);
                Vector2 origin = new Vector2(OUTER_RADIUS * Mathf.Cos(i * angleIncrement + angleVariance), OUTER_RADIUS * Mathf.Sin(i * angleIncrement + angleVariance));
                float distanceFactor = Random.Range(INNER_RADIUS, OUTER_RADIUS) / OUTER_RADIUS;
                temp.transform.position = distanceFactor * origin;
                temp.RpcInitialize();
            }
        }
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
                poolItem.RpcTerminate();
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
            pools.Add(newPool.GetComponent<Pool>());
        }
    }

    private void CreatePoolItems(Pool pool, int quantity)
    {
        int neededQty = quantity - pool.CheckInventory();
        if (neededQty > 0)
        {
            pool.InstantiatePoolObjects(neededQty);
        }
    }
}
