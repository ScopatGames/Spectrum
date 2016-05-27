using UnityEngine;

public class SmoothCameraPlanet : MonoBehaviour {
    public float lagTime = 12f;
    public Transform player;
    public float rotateSpeed = 10;
    public float radialOffset = 8;
    public float upperBoundary = 75;
    public float lowerBoundary = 25;

    private Vector3 from;
    private Vector3 to;
    private float targetAngle;
    private Vector3 newPos;
    private float cachedMagnitude;

    void FixedUpdate()
    {
        if (player)
        {
            from = transform.position;
            cachedMagnitude = player.position.magnitude;
            to = player.position*(cachedMagnitude-radialOffset)/cachedMagnitude;
            to.z = transform.position.z;
            transform.position -= (from - to) * lagTime * Time.deltaTime;

            targetAngle = Mathf.Rad2Deg * Mathf.Atan2(-transform.position.x, transform.position.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotateSpeed * Time.deltaTime);
        }

        newPos = new Vector3(transform.position.x, transform.position.y, 0f);
       
        if (newPos.sqrMagnitude > upperBoundary * upperBoundary)
        {
            newPos = newPos.normalized * upperBoundary;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else if (newPos.sqrMagnitude < lowerBoundary * lowerBoundary)
        {
            newPos = newPos.normalized * lowerBoundary;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }
}
