using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerPlanetDefenseSP : MonoBehaviour {

    public float addedThrustForce = 50;
    public float outerBoundaryRadius = 80;
    public float innerBoundaryRadius = 10;
    public float rotateSpeed = 3;

    private bool input;
    private float inputAngle;
    private float targetAngle;
    private Vector3 inputVector;
    private float thrustFactor;
    private Vector3 directionVector;
    private Rigidbody2D rigidBody2D;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Get player inputs
        inputVector = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f);
        //Test for input threshold:
        input = (inputVector.sqrMagnitude > 0.01) ? true : false;

        //If user is supplying input, calculate thrust factor
        thrustFactor = inputVector.sqrMagnitude * addedThrustForce;

        //Calculate the user input angle
        inputAngle = Mathf.Rad2Deg * Mathf.Atan2(inputVector.y, inputVector.x);

        //Add thrust force to player
        rigidBody2D.AddForce(transform.right * thrustFactor);

        
        //Calculate the direction of no-input flight vector
        directionVector = Vector3.Cross(transform.position, new Vector3(0, 0, 1));


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
        

        ///////////BOUNDARY CONTROL/////////////
        //Check to see if player is within playable boundary and update if necessary:

        Vector3 newPos;
        if (transform.position.sqrMagnitude > outerBoundaryRadius * outerBoundaryRadius)
        {
            newPos = transform.position.normalized * outerBoundaryRadius;
            newPos.z = 0f;
            transform.position = newPos;
        }
        else if(transform.position.sqrMagnitude < innerBoundaryRadius * innerBoundaryRadius)
        {
            newPos = transform.position.normalized * innerBoundaryRadius;
            newPos.z = 0f;
            transform.position = newPos;
        }
        ////////////END BOUNDARY CONTROL/////////////////
    }
}
