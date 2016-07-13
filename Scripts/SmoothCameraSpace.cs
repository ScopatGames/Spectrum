using UnityEngine;
using System.Collections;

public class SmoothCameraSpace : MonoBehaviour {

	public float lagTime = 12f;
	public Transform target;
    [Header("Enter the half width and height of level in world units")]
    public Vector2 levelExtents;

    private Camera cam;
    public Vector2 cameraExtents;


    void Start()
    {
        cam = GetComponent<Camera>();
        Vector3 cameraWorldPointZeroZero = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        cameraExtents = new Vector2(levelExtents[0] - Mathf.Abs(cameraWorldPointZeroZero.x - transform.position.x), levelExtents[1] - Mathf.Abs(cameraWorldPointZeroZero.y - transform.position.y));
    }


	void FixedUpdate () {
        if (target)
        {
            Vector3 from = transform.position;
            Vector3 to = target.position;
            to.z = transform.position.z;
            transform.position -= (from - to) * lagTime * Time.deltaTime;
        }
        float newXPos;
        float newYPos; 

        if(transform.position.x > cameraExtents[0])
        {
            newXPos = cameraExtents[0];
        }
        else if(transform.position.x < -cameraExtents[0])
        {
            newXPos = -cameraExtents[0];
        }
        else
        {
            newXPos = transform.position.x;
        }

        if (transform.position.y > cameraExtents[1])
        {
            newYPos = cameraExtents[1];
        }
        else if (transform.position.y < -cameraExtents[1])
        {
            newYPos = -cameraExtents[1];
        }
        else
        {
            newYPos = transform.position.y;
        }
        Vector3 newPos = new Vector3(newXPos, newYPos, transform.position.z);
        transform.position = newPos;

    }


}
