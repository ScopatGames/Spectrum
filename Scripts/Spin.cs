using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	public float speed = 10f;
	public Vector3 rotateVector;
    public bool randomSpin = false;
	
    void Start()
    {
        if (randomSpin)
        {
            rotateVector = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            speed = Random.Range(200f, 400f);
        }
    }
	
	void Update ()
	{
		transform.Rotate(rotateVector, speed * Time.deltaTime);
	}
}
