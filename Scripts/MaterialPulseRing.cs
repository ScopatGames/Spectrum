using UnityEngine;
using System.Collections;

public class MaterialPulseRing : MonoBehaviour {

    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        rend.material.SetFloat("_Color02Radius", Mathf.PingPong(Time.time/Random.Range(5f, 10f), 0.005f)+0.46f);
	}
}
