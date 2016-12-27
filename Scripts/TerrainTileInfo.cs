using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTileInfo : MonoBehaviour {

    public int tileIndex;
    public PolygonCollider2D polygonCollider2D;
    public MeshRenderer meshRenderer;

    void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void DestroyTile()
    {
        polygonCollider2D.enabled = false;
        meshRenderer.enabled = false;
    }

}
