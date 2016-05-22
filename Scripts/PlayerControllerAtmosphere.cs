using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PlayerControllerAtmosphere : MonoBehaviour {
    public float thrustForce = 100;
    public float addedThrustForce = 50;
    public float maxVelocity = 100;
    public float outerBoundaryRadius = 120;
    public float innerBoundaryRadius = 80;
    public float rotateSpeedNoInput = 500;
    public float rotateSpeedWithInput = 10;
    public float rollSpeed = 3;
    public float rotationInputSensitivity = 100;
    public float moveCounter;
    public float moveCounterReset = 0.3f;
    public GameObject barrierIndicator;
    public Transform childRollTransform;

    private Renderer barrierRenderer;
    private Color barrierColor;
    private float minX=0.3f;
    private float minY=0.15f;
    private bool xInput;
    private bool yInput;
    private bool flyingClockwise;
    private Vector3 gravityVector;
    private float targetAngleZ;
    private int rollCount = 0;
    private Quaternion childRotationTarget;
    private float controlDirection = -1f;
    private float rotateSpeed;
    public float rotateSpeedActual;
    private bool yInputLastUpdate;
    private Vector3 directionVector;




    Rigidbody2D rigidBody2D;
    void Start()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
        CrossPlatformInputManager.SetButtonUp("Next");
        rotateSpeedActual = rotateSpeedNoInput;
    }

    void FixedUpdate()
    {
        //This dampens the rotation when the joystick is released
        if (rotateSpeedActual < rotateSpeed)
        {
       
            rotateSpeedActual = (rotateSpeedActual*1.1f > rotateSpeed) ? rotateSpeed : rotateSpeedActual * 1.1f;
            
        }

        //Determine the current direction of flight
        flyingClockwise = (Vector3.Cross(transform.position, transform.right).z < 0) ? true : false;
      
        //Get player inputs
        Vector3 inputVector = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f);
        //Test for input threshold:
        xInput = (Mathf.Abs(inputVector.x) > minX) ? true : false;
        yInput = (Mathf.Abs(inputVector.y) > minY) ? true : false;
        //Reset thrust vectors:
        Vector3 thrustVector = Vector3.zero;
        Vector3 addedThrustVector = Vector3.zero;
        
                
        
        //Thrust input
        if(xInput)
        {
            //Calculate addedThrustVector here...
            addedThrustVector = transform.right * inputVector.x * Mathf.Abs(inputVector.x) * addedThrustForce;
            if (!flyingClockwise)
            {
                addedThrustVector = -1 * addedThrustVector;
            }
        }
        
        //Rotation input
        if (yInput)
        {
            //New, simple method:
            thrustVector = transform.right * thrustForce;
            if (!yInputLastUpdate)
            {
                rotateSpeed = rotateSpeedWithInput;
                rotateSpeedActual = rotateSpeed;
            }
            yInputLastUpdate = true;
            //Calculate angle due to player input
            float inputAngle = controlDirection*inputVector.y * Mathf.Abs(inputVector.y) * rotationInputSensitivity;

            //Factor in component due to pitch
            targetAngleZ = transform.rotation.eulerAngles.z + inputAngle;
        }
        else
        {
            if (yInputLastUpdate)
            {
                rotateSpeedActual = 1f;
                rotateSpeed = rotateSpeedNoInput;
            }
            yInputLastUpdate = false;
            //New simple method:
            thrustVector = transform.right * thrustForce;

            //Correct angle for camera rotation only
            directionVector = Vector3.Cross(transform.position, new Vector3(0, 0, 1)).normalized;
            if (!flyingClockwise)
            {
                directionVector = -1 * directionVector;
            }
            targetAngleZ = Mathf.Rad2Deg * Mathf.Atan2(directionVector.y, directionVector.x);
        }
        
        //Update player rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngleZ), rotateSpeedActual * Time.deltaTime);
        //Update roll rotation
        childRollTransform.localRotation = Quaternion.Slerp(childRollTransform.localRotation, childRotationTarget, rollSpeed*Time.deltaTime);
 
        //add constant forward force plus additional forces
        gravityVector = transform.position * -.05f * rigidBody2D.mass;
        rigidBody2D.AddForce(thrustVector + addedThrustVector + gravityVector);

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
        else if(transform.position.sqrMagnitude < innerBoundaryRadius * innerBoundaryRadius){
            newPos = transform.position.normalized * innerBoundaryRadius;
            newPos.z = 0f;
            transform.position = newPos;

            //Move Barrier Indicator
            barrierIndicator.transform.position = transform.position;
            barrierIndicator.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(-barrierIndicator.transform.position.x, barrierIndicator.transform.position.y)+180);
        }
        else
        {
            barrierIndicator.transform.position = new Vector3(1000f, 0f, 0f);
        }
        ////////////END BOUNDARY CONTROL/////////////////
    }

    void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Next"))
        {
            SceneManager.LoadScene(2); 
            //Application.LoadLevel(2);
        }

        //Check to see if the player is flying upside-down on joystick button release
        if (CrossPlatformInputManager.GetButtonUp("JoystickButton"))
        {
            if (Vector3.Dot(childRollTransform.up, transform.position) < 0f)
            {
                //player is upsidedown and needs to roll
                if (rollCount % 2 == 0)
                {
                    //rollAngle = childTransform.rotation.eulerAngles.x + 180f;
                    childRotationTarget = Quaternion.Euler(180f, 0f, 0f);
                }
                else
                {
                    //rollAngle = childTransform.rotation.eulerAngles.y - 180f;
                    childRotationTarget = Quaternion.Euler(0f, 0f, 0f);
                }
                rollCount++;
                controlDirection *= -1f;
            }
        }
    }
}
