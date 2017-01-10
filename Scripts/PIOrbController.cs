using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PIOrbController : PoolItem {
    private Spin spin;
    private MeshRenderer meshRenderer;
    private CircleCollider2D circleCollider2D;

    public override void OnStartClient()
    {
        base.OnStartClient();
        spin = GetComponent<Spin>();
        meshRenderer = GetComponent<MeshRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        Terminate();

    }

    [ClientRpc]
    public override void RpcInitialize()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        spin.enabled = true;
        meshRenderer.enabled = true;
        circleCollider2D.enabled = true;
    }
    
    [ClientRpc]
    public override void RpcTerminate()
    {
        Terminate();
    }

    public override void Terminate()
    {
        base.Terminate();
        spin.enabled = false;
        meshRenderer.enabled = false;
        circleCollider2D.enabled = false;
    }
}
