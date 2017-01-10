using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PIBombController : PoolItem {
    public float blastRadius;
    private bool hitSomething;
    private MeshRenderer meshRenderer;
    private CircleCollider2D circleCollider2D;
    private Gravity gravity;
    private Spin spin;
    private Rigidbody2D rb;

    public override void OnStartClient()
    {
        base.OnStartClient();
        meshRenderer = GetComponent<MeshRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        gravity = GetComponent<Gravity>();
        spin = GetComponent<Spin>();
        rb = GetComponent<Rigidbody2D>();
        Terminate();
    }
    [ServerCallback]
	void OnCollisionEnter2D(Collision2D collision2D)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach(Collider2D hit in hitColliders)
        {
            if (hit.tag == _Tags.environment)
            {
                GameManagerMultiplayer.instance.CmdDestroyTerrainTile(hit.GetComponent<TerrainTileInfo>().tileIndex);
                hitSomething = true;
            }
        }

        if (hitSomething)
        {
            GameManagerMultiplayer.instance.itemController.deployedItems.Remove(this);
            GetComponent<PoolItem>().pool.CheckIn(GetComponent<PoolItem>());
        }

        
    }

    [ClientRpc]
    public void RpcSetState(Vector3 position, Vector2 velocity)
    {
        transform.position = position;
        rb.velocity = velocity;
    }

    [ClientRpc]
    public override void RpcInitialize()
    {
        Initialize();

    }

    public override void Initialize()
    {
        meshRenderer.enabled = true;
        circleCollider2D.enabled = true;
        gravity.enabled = true;
        spin.enabled = true;
        rb.isKinematic = false;
    }

    [ClientRpc]
    public override void RpcTerminate()
    {
        Terminate();
    }
    public override void Terminate()
    {
        meshRenderer.enabled = false;
        circleCollider2D.enabled = false;
        gravity.enabled = false;
        spin.enabled = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
