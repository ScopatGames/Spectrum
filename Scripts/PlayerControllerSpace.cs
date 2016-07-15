using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerSpace : NetworkBehaviour {
    public float thrustForce = 5;
    public float maxVelocity = 10;
    public float boundaryRadius = 40;
    public float rotateSpeed = 10;
    public float moveCounter;
    public float moveCounterReset = 0.3f;

    private Vector3 inputVector;
    private float targetAngle;
    private Rigidbody2D rigidBody2D;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {

            inputVector = Vector3.ClampMagnitude(new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f), 1.0f);

            if (inputVector.sqrMagnitude > 0.01)
            {
                if (rigidBody2D.velocity.sqrMagnitude < maxVelocity * maxVelocity)
                {
                    rigidBody2D.AddForce(transform.up * thrustForce * inputVector.sqrMagnitude);
                }

                targetAngle = Mathf.Rad2Deg * Mathf.Atan2(-inputVector.x, inputVector.y);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotateSpeed * Time.deltaTime);
            }


            ///////////BOUNDARY CONTROL/////////////
            //Check to see if player is within playable boundary and update if necessary:
            Vector3 newPos;
            if (transform.position.sqrMagnitude > boundaryRadius * boundaryRadius)
            {
                if (isLocalPlayer)
                {
                    newPos = transform.position.normalized * boundaryRadius;
                    newPos.z = 0f;
                    transform.position = newPos;
                }
            }
            ////////////END BOUNDARY CONTROL/////////////////
        }
    }


}
