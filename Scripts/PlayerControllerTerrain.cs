using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerTerrain : MonoBehaviour {
    public float thrustForce = 5;
    public float jumpForce = 5;
    public float maxVelocity = 10;
    public float boundaryRadius = 40;
    public float rotateSpeed = 10;
    public float moveCounter;
    public float moveCounterReset = 0.3f;
    public GameObject barrierIndicator;
    public float rotateSpeedActual = 100f;
    public Transform childPivotTransform;
    public float pivotSpeed = 3f;
    public bool lookingRight = true;

    private float minX = 0.3f;
    private float minY = 0.15f;
    private bool xInput;
    private bool yInput;
    private float targetAngleZ;
    private Vector3 jumpVector;
    private Vector3 gravityVector;
    private Vector3 thrustVector;
    private Vector3 directionVector;
    private Quaternion childRotationTarget;
    private Renderer barrierRenderer;
    private Color barrierColor;
    



    Rigidbody2D rigidBody2D;
    void Start()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
        CrossPlatformInputManager.SetButtonUp("Next");
    }

    void FixedUpdate()
    {
        //Get player inputs
        Vector3 inputVector = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f);
        //Test for input threshold:
        xInput = (Mathf.Abs(inputVector.x) > minX) ? true : false;
        yInput = (Mathf.Abs(inputVector.y) > minY) ? true : false;
        thrustVector = Vector3.zero;
        targetAngleZ = 0f;
        jumpVector = Vector3.zero;
        
        
        //Thrust input
        if (xInput)
        {
            //Calculate thrustVector here...
            thrustVector = transform.right * inputVector.x * Mathf.Abs(inputVector.x) * thrustForce;
            
        }

        if (yInput)
        {
            if (inputVector.y > 0)
            {
                jumpVector = transform.up * inputVector.y*Mathf.Abs(inputVector.y)*jumpForce;
            }
        }

        gravityVector = transform.position * -3f * rigidBody2D.mass;
        rigidBody2D.AddForce(thrustVector + gravityVector + jumpVector);

        //Correct angle for camera rotation only
        directionVector = Vector3.Cross(transform.position, new Vector3(0, 0, 1)).normalized;
        targetAngleZ = Mathf.Rad2Deg * Mathf.Atan2(directionVector.y, directionVector.x);
        
        //Update player rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngleZ), rotateSpeedActual * Time.deltaTime);

        //Update roll rotation
        childPivotTransform.localRotation = Quaternion.Slerp(childPivotTransform.localRotation, childRotationTarget, pivotSpeed * Time.deltaTime);

        /*OLD SPACE CONTROLS>>>>>
        Vector3 inputVector = Vector3.ClampMagnitude(new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f), 1.0f);

        if (inputVector.sqrMagnitude > 0.01)
        {
            if (rigidBody2D.velocity.sqrMagnitude < maxVelocity * maxVelocity)
            {
                rigidBody2D.AddForce(transform.up * thrustForce * inputVector.sqrMagnitude);
            }
            else
            {
                Debug.Log("already at max velocity");
            }

            float targetAngle = Mathf.Rad2Deg * Mathf.Atan2(-inputVector.x, inputVector.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotateSpeed * Time.deltaTime);


        <<<<<OLD SPACE CONTROLS}*/

        //Check to see if player is within playable boundary and update if necessary:
        Vector3 newPos;

        if (transform.position.sqrMagnitude > boundaryRadius * boundaryRadius)
        {
            newPos = transform.position.normalized * boundaryRadius;
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
    }

    void Update()
    {

        if (CrossPlatformInputManager.GetButtonDown("Next"))
        {
            Application.LoadLevel(0);
        }
        if (CrossPlatformInputManager.GetAxis("Horizontal") > 0f)
        {
            childRotationTarget = Quaternion.Euler(0f, 0f, 0f);
            lookingRight = true;
        }
        else if (CrossPlatformInputManager.GetAxis("Horizontal") < 0f)
        {
            childRotationTarget = Quaternion.Euler(0f, 70f, 0f);
            lookingRight = false;
        }
    }
}
