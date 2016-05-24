using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerPlanet : MonoBehaviour
{
    public float defaultThrustFactor = 0.3f;
    public float addedThrustForce = 50;
    public float outerBoundaryRadius = 80;
    public float rotateSpeed = 3;
    public float rollSpeed = 3;
    public GameObject barrierIndicator;
    public Transform childRollTransform;

    private float minX = 0.5f;
    private float minY = 0.15f;
    private bool xInput;
    private bool yInput;
    private bool flyingClockwise = true;
    private bool flyingClockwiseLastFrame = true;
    private float targetAngleZ;
    private bool rollIsAvailable = true;
    private int rollCount = 0;
    private Quaternion childRotationTarget;
    private Vector3 inputVector;
    private Vector3 thrustVector;
    private Vector3 xThrust;
    private Vector3 yThrust;
    private Vector3 directionVector;
    private Rigidbody2D rigidBody2D;

    void Start()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();

        //Instantiate barrier
        barrierIndicator = Instantiate(barrierIndicator, new Vector3(1000, 0, 0), Quaternion.identity) as GameObject;
    }

    void FixedUpdate()
    {
        
        //Get player inputs
        inputVector = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f);
        //Test for input threshold:
        xInput = (Mathf.Abs(inputVector.x) > minX) ? true : false;
        yInput = (Mathf.Abs(inputVector.y) > minY) ? true : false;
        //Reset thrust vectors:
        thrustVector = Vector3.zero;
        xThrust = Vector3.zero;
        yThrust = Vector3.zero;

        //Determine the current direction of flight
        flyingClockwiseLastFrame = flyingClockwise;
        flyingClockwise = (Vector3.Cross(transform.position, transform.right).z < 0) ? true : false;

        if (xInput)
        {
            xThrust = Math.Abs(inputVector.x) * transform.right;
        }
        else
        {
            xThrust = transform.right * defaultThrustFactor;
        }

        if(yInput)
        {
            yThrust = inputVector.y * transform.up;
            if (!flyingClockwise)
            {
                yThrust = -1 * yThrust;
            }
        }

        thrustVector = xThrust * addedThrustForce;
        rigidBody2D.AddForce(thrustVector);

        //Correct angle for camera rotation only
        directionVector = Vector3.Cross(transform.position, new Vector3(0, 0, 1)).normalized;

        //if (inputVector.x < 0 || (inputVector.x == 0 && !flyingClockwiseLastFrame))
        if ((xInput && inputVector.x < 0) || (!xInput && !flyingClockwiseLastFrame))
        {
            directionVector = -1 * directionVector;
        }

        targetAngleZ = Mathf.Rad2Deg * Mathf.Atan2(directionVector.y, directionVector.x);

        if (yInput)
        {
            targetAngleZ = flyingClockwise ? targetAngleZ + inputVector.y * 70 : targetAngleZ - inputVector.y * 70;
        }

        //Update player rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngleZ), rotateSpeed * Time.deltaTime);
        //Update roll rotation
        childRollTransform.localRotation = Quaternion.Slerp(childRollTransform.localRotation, childRotationTarget, rollSpeed * Time.deltaTime);

        ///////////BOUNDARY CONTROL/////////////
        //Check to see if player is within playable boundary and update if necessary:
        Vector3 newPos;
        if (transform.position.sqrMagnitude > outerBoundaryRadius * outerBoundaryRadius)
        {
            newPos = transform.position.normalized * outerBoundaryRadius;
            newPos.z = 0f;
            transform.position = newPos;

            //Move Barrier Indicator
            barrierIndicator.transform.position = transform.position;
            barrierIndicator.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(-barrierIndicator.transform.position.x, barrierIndicator.transform.position.y));
        }
        else
        {
            barrierIndicator.transform.position = new Vector3(1000f, 0f, 0f);
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
