using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour {

    [HideInInspector]
    public Pool pool;

    public virtual void Initialize()
    {

    }

    public virtual void Terminate()
    {

    }
	
}
