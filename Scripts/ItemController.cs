using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ItemController : NetworkBehaviour {
    [Header("Item Prefabs by Deployment Method (increasing capability)")]
    public List<PoolItem> groundDefenses = new List<PoolItem>();
    public List<PoolItem> neutralPickups = new List<PoolItem>();

    private List<Pool> groundDefensesPools = new List<Pool>();
    private List<Pool> neutralPickupsPools = new List<Pool>();
    private List<PoolItem> deployedItems = new List<PoolItem>();

    void Awake()
    {
        CreatePools(groundDefenses, groundDefensesPools);
        CreatePools(neutralPickups, neutralPickupsPools);
    }

    //PUBLIC METHODS
    //--------------------------------------------------------
    public void DeployGroundDefenses(int level, int quantity)
    {
        Pool poolRef = groundDefensesPools[level];
        CreatePoolItems(poolRef, quantity);

        GroundEvenDistribution(poolRef, quantity);

    }

    //PRIVATE METHODS
    //--------------------------------------------------------
    private void CreatePools(List<PoolItem> inputList, List<Pool> outputList)
    {
        foreach (PoolItem prefab in inputList)
        {
            GameObject newPool = new GameObject("Pool ("+prefab.gameObject.name + ")");
            Pool poolComponent = newPool.AddComponent<Pool>();
            poolComponent.InitializePool(prefab);
            outputList.Add(poolComponent);
        }
    }

    private void CreatePoolItems(Pool pool, int quantity)
    {
        if (pool.CheckInventory() < quantity)
        {
            if (pool.poolObjectPrefab.GetComponent<NetworkIdentity>())
            {
                pool.RpcSpawnPoolObjects(quantity - pool.CheckInventory());
            }
            else
            {
                pool.InstantiatePoolObjects(quantity - pool.CheckInventory());
            }
        }
    }

    private void DeployNeutralPickups(int level, int quantity)
    {
        CreatePoolItems(neutralPickupsPools[level], quantity);
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
}
