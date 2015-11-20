using UnityEngine;
using System.Collections;

public class CubePickup : MonoBehaviour {

    private Animator anim;
    private AnimatorControllerParameter acp;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void OnTriggerEnter2D()
    {
        anim.SetTrigger("pickup");
    }

    void DestroyCube()
    {
        Destroy(gameObject);
        GetComponentInParent<PickupCounter>().incrementCounter();
    }
}
