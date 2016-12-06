using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	void FixedUpdate () {
        rb.AddForce((Vector3.zero - transform.position) * rb.mass*0.1f);
	}
}
