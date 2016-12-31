using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIOrbController : PoolItem {
    private Spin spin;
    private MeshRenderer meshRenderer;
    private CircleCollider2D circleCollider2D;

    void Awake()
    {
        spin = GetComponent<Spin>();
        meshRenderer = GetComponent<MeshRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public override void Initialize()
    {
        spin.enabled = true;
        meshRenderer.enabled = true;
        circleCollider2D.enabled = true;
    }

    public override void Terminate()
    {
        spin.enabled = false;
        meshRenderer.enabled = false;
        circleCollider2D.enabled = false;
    }
}
