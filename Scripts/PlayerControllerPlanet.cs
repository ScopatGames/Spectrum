using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerPlanet : NetworkBehaviour
{
    public float defaultThrustFactor = 0.3f;
    public float addedThrustForce = 50;
    public float outerBoundaryRadius = 80;
    public float rotateSpeed = 3;
    public float rollSpeed = 3;
    public Transform childRollTransform;

    private bool input;
    private bool flyingClockwise = true;
    private bool flyingClockwiseLastFrame = true;
    private float inputAngle;
    private float targetAngle;
    private bool rollIsAvailable = true;
    private int rollCount = 0;
    private Quaternion childRotationTarget;
    private Vector3 inputVector;
    private float thrustFactor;
    private Vector3 directionVector;
    private Rigidbody2D rigidBody2D;

    void Awake()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            //Get player inputs
            inputVector = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f);
            //Test for input threshold:
            input = (inputVector.sqrMagnitude > 0.01) ? true : false;

            //Determine the current direction of flight
            flyingClockwiseLastFrame = flyingClockwise;
            flyingClockwise = (Vector3.Cross(transform.position, transform.right).z < 0) ? true : false;

            if (input)
            {
                //If user is supplying input, calculate thrust factor
                thrustFactor = (inputVector.sqrMagnitude + defaultThrustFactor) * addedThrustForce;

                //Calculate the user input angle
                inputAngle = Mathf.Rad2Deg * Mathf.Atan2(inputVector.y, inputVector.x);
            }
            else
            {
                //Use default thrust factor if no input
                thrustFactor = defaultThrustFactor * addedThrustForce;
            }

            //Add thrust force to player
            rigidBody2D.AddForce(transform.right * thrustFactor);

            //Calculate the direction of no-input flight vector
            directionVector = Vector3.Cross(transform.position, new Vector3(0, 0, 1));

            //If player is flying counterclockwise reverse the direction vector and input angles
            if ((input && inputVector.x < 0) || (!input && !flyingClockwiseLastFrame))
            {
                directionVector = -1 * directionVector;
                inputAngle += 180f;
            }

            //Determine target angle based on input or no-input
            if (input)
            {
                targetAngle = Mathf.Rad2Deg * Mathf.Atan2(directionVector.y, directionVector.x) + inputAngle;
            }
            else
            {
                targetAngle = Mathf.Rad2Deg * Mathf.Atan2(directionVector.y, directionVector.x);
            }

            //Update player rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotateSpeed * Time.deltaTime);
            //Update roll rotation
            childRollTransform.localRotation = Quaternion.Slerp(childRollTransform.localRotation, childRotationTarget, rollSpeed * Time.deltaTime);
        }
        ///////////BOUNDARY CONTROL/////////////
        //Check to see if player is within playable boundary and update if necessary:

        Vector3 newPos;
        if (transform.position.sqrMagnitude > outerBoundaryRadius * outerBoundaryRadius)
        {
            if (isLocalPlayer)
            {
                newPos = transform.position.normalized * outerBoundaryRadius;
                newPos.z = 0f;
                transform.position = newPos;
            }
        }
        ////////////END BOUNDARY CONTROL/////////////////
    }

    void Update()
    {
        //Check to see if the player is flying upside-down
        if (rollIsAvailable && Vector3.Dot(childRollTransform.up, transform.position) < 0f)
        {
            //player is upsidedown and needs to roll
            if (rollCount % 2 == 0)
            {
                childRotationTarget = Quaternion.Euler(180f, 0f, 0f);
            }
            else
            {
                childRotationTarget = Quaternion.Euler(0f, 0f, 0f);
            }
            rollCount++;
            rollIsAvailable = false;
            Invoke("resetRollTimer", 1);
        }
    }

    private void resetRollTimer()
    {
        rollIsAvailable = true;
    }

}
