using UnityEngine;

public class PlayerCharCont : MonoBehaviour
{
    CharacterController charCont;

    private Vector3 moveDirection = Vector3.zero;   //Creates a new vector3 variable that will be used to define how the player will move. Gives it an initial value of zero for all three values.
    public float speed;                       //Creates a new float variable that will store a speed modifier for the player. "f" denotes the number as a float value. "public" will create a little box in the inspector that you can modify.
    public float sprintSpeed;
    private float startSpeed;                       //This will hold the initial speed, so we know what value we should set speed to when the player finishes sprinting.
    public float gravity = 20f;
    public float jumpSpeed = 8f;
    //public Animator anim;

    //public Rigidbody projectile;    //New variable to hold the rigidbody gameobject that will be used as a projectile
    public float shootSpeed;
    public float pushSpeed;

    //knockback variables
    private bool knockBack = false;
    private int kbCount = 0;
    public int kbSpeed;
    public int kbDistance;

    private void Start()        //Anything here will happen at the start of the scene.
    {
        charCont = GetComponent<CharacterController>(); //Finds and stores the reference to the character controller component
        startSpeed = speed;     //set startspeed to speed, which is what we set in the inspector. We can use this to reset speed when player finishes sprinting.

    }


    void Update()              //Anything here happens once per frame.
    {
        PlayerMove();           //Call the function that handles movement

        if (knockBack)
        {

            charCont.Move(transform.forward*kbSpeed*Time.deltaTime);
            kbCount++;
            if (kbCount > kbDistance)
            {
                knockBack = false;
            }

            
        }

        /*if (Input.GetButtonDown("Fire1"))       //If Left mouse button is clicked, shoot.
        {
            PlayerShoot();      //Call the function that handles shooting
        }*/


    }
    
    void PlayerMove()           //Handles all player movement
    {
        if (charCont.isGrounded)    //Checks to see if the player is on the ground.
        {   //Everything in here runs ONLY if the player is on the ground during the current frame.
            float moveX = Input.GetAxis("Horizontal");  //Stores keyboard input into float variables
            float moveZ = Input.GetAxis("Vertical");

            /*if (moveZ != 0 || moveX != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            Debug.Log(moveX);

            anim.SetFloat("Direction", moveX);
            anim.SetFloat("Speed", moveZ);
            anim.SetBool("isJumping", false);*/


            if (Input.GetButton("Sprint"))
            {
                speed = sprintSpeed;        //Adjust speed for sprinting
            }
            else
            {
                speed = startSpeed;         //Reset speed back to start speed.
            }

            //Converts keyboard input stored in moveX and moveZ to a Vector3, so it can be used for movement.
            //Exercise: try making moveDirection = transform.right; Remove everything else. See what happens. Now try to make it transform.forward. See what happens. Enable both. Enable one, and put a zero. Enable one and put 100. Play around with this code and get a feel for what these do.
            moveDirection = transform.right * moveX + transform.forward * moveZ; //transform.right gets a numerical value for left/right movement. Forward gets forward/back movement. Multiply each by the input for X (horizontal) and Z (vertical) to build a Vector 3 for movement. Does not actually move the player yet.          
            moveDirection *= speed;                                              //Speed multiplier for movement.


            if (Input.GetButton("Jump"))        //If the jump button is pressed.
            {
                moveDirection.y = jumpSpeed;    //Adjust the y coordinates of the moveDirection Vector3 variable when the jump button is pressed.
                //anim.SetBool("isJumping", true);
            }

        }

        //This is where we apply gravity. Gravity is multipled by Time.deltaTime twice - here and below where we actually move the character controller. This is because gravity accelerates, and is defined in terms of meters per second squared. The squared part is why we multiply by deltaTime twice.  
        moveDirection.y -= gravity * Time.deltaTime;
        charCont.Move(moveDirection * Time.deltaTime);  //Actually moves the GameObject with the character controller on it.
    }

    /*void PlayerShoot()          //Handles player shooting
    {
        Debug.Log("shoot");
        
        Rigidbody clone;
        Vector3 startPosition;

        startPosition = transform.position;
        startPosition.y += 1.4f;

        clone = Instantiate(projectile, startPosition + transform.forward, transform.rotation);

        clone.velocity = transform.forward * shootSpeed;

    }*/




    /*********************
     ***** Collisions*****
     ********************/

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;    //Gets the rigidbody of the thing the character collided with.

        if (body == null || body.isKinematic) //if there is no rigidbody, or its "is kinematic" checkbox is checked
        {
            return;                           //Stop executing the onControllerColliderHit. This ends the function immediately, and does not execute the lines after this one. If the thing we collided with has "is kinematic" checked, there's nothing more we need to do here. We don't want to affect it!
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push
        body.velocity = pushDir * pushSpeed;
    }

    public void KnockBack()
    {
        kbCount = 0;
        knockBack = true;           
    }

}
