using UnityEngine;
using System.Collections;

public class PlayerPickup : MonoBehaviour {

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
        Destroy(gameObject); //TODO: Edit this to change state of opponent
    }
}
