using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItem : MonoBehaviour {
    [HideInInspector]
    public Pool pool;

    public abstract void Initialize();

    public abstract void Terminate();
	
}
