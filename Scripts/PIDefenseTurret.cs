using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PIDefenseTurret : PoolItem {

    [ClientRpc]
    public override void RpcInitialize()
    {
        Initialize();
    }

    public override void Initialize()
    {
    }

    [ClientRpc]
    public override void RpcTerminate()
    {
        Terminate();
    }

    public override void Terminate()
    {
    }
}
