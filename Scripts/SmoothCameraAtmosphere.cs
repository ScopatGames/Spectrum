using UnityEngine;
using System.Collections;

public class SmoothCameraAtmosphere : MonoBehaviour {
    public float lagTime = 12f;
    public Transform player;
    public float rotateSpeed = 10;
    public float radialOffset = 8;
    public float upperBoundary = 75;
    public float lowerBoundary = 25;
    // [Header("Enter the half width and height of level in world units")]
    // public Vector2 levelExtents;

    private Vector3 newPos;
    //private Camera cam;
    //public Vector2 cameraExtents;


    void Start()
    {
        //cam = GetComponent<Camera>();
        //Vector3 cameraWorldPointZeroZero = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        //cameraExtents = new Vector2(levelExtents[0] - Mathf.Abs(cameraWorldPointZeroZero.x - transform.position.x), levelExtents[1] - Mathf.Abs(cameraWorldPointZeroZero.y - transform.position.y));
    }


    void FixedUpdate()
    {
        if (player)
        {
            Vector3 from = transform.position;
            Vector3 to = player.position.normalized*(player.position.magnitude-radialOffset);
            to.z = transform.position.z;
            transform.position -= (from - to) * lagTime * Time.deltaTime;

            float targetAngle = Mathf.Rad2Deg * Mathf.Atan2(-transform.position.x, transform.position.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotateSpeed * Time.deltaTime);
        }

        Vector3 testPos = new Vector3(transform.position.x, transform.position.y, 0f);
       
        if (testPos.sqrMagnitude > upperBoundary * upperBoundary)
        {
            newPos = testPos.normalized * upperBoundary;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else if (testPos.sqrMagnitude < lowerBoundary * lowerBoundary)
        {
            newPos = testPos.normalized * lowerBoundary;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        /* Sets camera to bounds of play area - rework for atmosphere
        float newXPos;
        float newYPos;

        if (transform.position.x > cameraExtents[0])
        {
            newXPos = cameraExtents[0];
        }
        else if (transform.position.x < -cameraExtents[0])
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
        */

    }


}
