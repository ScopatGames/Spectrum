using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerPlanet : MonoBehaviour
{
    public float thrustForce = 100;
    public float addedThrustForce = 50;
    public float maxVelocity = 100;
    public float outerBoundaryRadius = 80;
    public float rotateSpeedNoInput = 500;
    public float rotateSpeedWithInput = 10;
    public float rollSpeed = 3;
    public float rotationInputSensitivity = 100;
    public float moveCounter;
    public float moveCounterReset = 0.3f;
    public GameObject barrierIndicator;
    public Transform childRollTransform;
    public float rotateSpeed = 10f;

    private Renderer barrierRenderer;
    private Color barrierColor;
    private float minX = 0.3f;
    private float minY = 0.15f;
    private bool xInput;
    private bool yInput;
    private bool flyingClockwise;
    private Vector3 gravityVector;
    private float targetAngle = 0;
    private int rollCount = 0;
    private Quaternion childRotationTarget;
    private float controlDirection = -1f;
    private bool yInputLastUpdate;
    private Vector3 directionVector;

    Rigidbody2D rigidBody2D;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector3 inputVector = Vector3.ClampMagnitude(new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0.0f), 1.0f);

        if (inputVector.sqrMagnitude > 0.01)
        {
            if (rigidBody2D.velocity.sqrMagnitude < maxVelocity * maxVelocity)
            {
                rigidBody2D.AddForce(transform.right * thrustForce * inputVector.sqrMagnitude);
            }
            else
            {
                Debug.Log("already at max velocity");
            }
            targetAngle = Mathf.Rad2Deg * Mathf.Atan2(-inputVector.x, inputVector.y);
        }

        //Update player rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), rotateSpeed * Time.deltaTime);
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
