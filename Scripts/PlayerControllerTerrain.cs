using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControllerTerrain : MonoBehaviour {
    public float thrustForce = 5;
    public float jumpForce = 5000;
    public float jumpRechargeTime = 0.2f;
    public float jumpDuration = 2;
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
    public LayerMask environmentLayerMask;

    private float minX = 0.3f;
    private float minY = 0.5f;
    private bool xInput;
    private float targetAngleZ;
    private float jumpTimer;
    public bool playerGrounded = false;
    public bool playerJumping = false;
    private float jumpDurationTimer = 0f;
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
        
        thrustVector = Vector3.zero;
        targetAngleZ = 0f;
        jumpVector = Vector3.zero;
        
        
        //Thrust input
        if (xInput)
        {
            //Calculate thrustVector here...
            thrustVector = transform.right * inputVector.x * Mathf.Abs(inputVector.x) * thrustForce;
            
        }

        //Test to see if the player is on the ground before jumping
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, -transform.up, 1.0f, environmentLayerMask);
        
        if(hit2D.collider != null)
        {
            jumpTimer += Time.fixedDeltaTime;
            if(jumpTimer > jumpRechargeTime && !playerJumping)
            {
                playerGrounded = true;
                jumpDurationTimer = 0f;
            }
        }
        else
        {
            jumpTimer = 0f;
        }

        if (inputVector.y > minY)
        {
            if (playerGrounded)
            {
                jumpVector = (transform.up+ 0.25f*transform.right * inputVector.x) *jumpForce;
                playerGrounded = false;
                playerJumping = true;
            }
            else if (playerJumping && jumpDurationTimer < jumpDuration)
            {
                jumpDurationTimer += Time.fixedDeltaTime;
                jumpVector = (transform.up+ 0.25f*transform.right*inputVector.x) * (jumpForce / (jumpDuration * jumpDuration))*(jumpDurationTimer - jumpDuration)*(jumpDurationTimer - jumpDuration);
            }
            else
            {
                playerJumping = false;
            }
            
        }
        

        gravityVector = transform.position * -3f * rigidBody2D.mass;
        if(jumpVector.sqrMagnitude < gravityVector.sqrMagnitude)
        {
            jumpVector = Vector3.zero;
        }
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
            SceneManager.LoadScene(0);
            //Application.LoadLevel(0);
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
