using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public float blastRadius;
    private bool hitSomething;

	void OnCollisionEnter2D(Collision2D collision2D)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach(Collider2D hit in hitColliders)
        {
            if (hit.tag == _Tags.environment)
            {
                hit.GetComponent<MeshRenderer>().enabled = false;
                hit.GetComponent<PolygonCollider2D>().enabled = false;
                hitSomething = true;
            }
        }

        if (hitSomething)
        {
            Destroy(gameObject);
        }

        
    }
}
