using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PoolItem : NetworkBehaviour {
    public Pool pool;

    [ClientRpc]
    public virtual void RpcInitialize() { }
    [ClientRpc]
    public virtual void RpcTerminate() { }
    public virtual void Initialize() { }
    public virtual void Terminate() { }
	
}
