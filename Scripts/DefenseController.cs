using UnityEngine;
using System.Collections;

public class DefenseController : MonoBehaviour {

    Collider2D col;
    MeshRenderer meshRenderer;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize()
    {
        col.enabled = true;
        meshRenderer.enabled = true;
    }
}
