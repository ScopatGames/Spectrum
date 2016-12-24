using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemController : MonoBehaviour {
    [Header("Item Prefabs by Deployment Method (increasing capability)")]
    public List<GameObject> groundDefenses = new List<GameObject>();
    public List<GameObject> neutralPickups = new List<GameObject>();

    private List<Pool> groundDefensesPools = new List<Pool>();
    private List<Pool> neutralPickupsPools = new List<Pool>();
    private List<GameObject> deployedItems = new List<GameObject>();

    void Awake()
    {
        CreatePools(groundDefenses, groundDefensesPools);
        CreatePools(neutralPickups, neutralPickupsPools);
    }


    private void DeployGroundDefenses(int level, int quantity, Color color)
    {
        CreatePoolItems(groundDefensesPools[level], quantity);
    }

    private void DeployNeutralPickups(int level, int quantity, Color color)
    {
        CreatePoolItems(neutralPickupsPools[level], quantity);
    }

    private void CreatePools(List<GameObject> inputList, List<Pool> outputList)
    {
        foreach (GameObject prefab in inputList)
        {
            GameObject newPool = new GameObject(prefab.name + " Pool");
            Pool poolComponent = newPool.AddComponent<Pool>();
            poolComponent.poolObjectPrefab = prefab;
            outputList.Add(poolComponent);
        }
    }

    private void CreatePoolItems(Pool pool, int quantity)
    {
        if(pool.CheckInventory() < quantity)
        {
            pool.InstantiatePoolObjects(quantity - pool.CheckInventory());
        }
    }
}
