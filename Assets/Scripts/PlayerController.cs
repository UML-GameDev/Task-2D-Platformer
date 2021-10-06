using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //speed player move and height player can jump
    public float speed = 5.0f;
    public float jumpHeight = 5.0f;
    public float dashDistance = 10.0f;

    //transformation that checks if player hits the ground
    public Transform ground;

    //How much force player needs to reach jump height
    private float jumpForce = 0.0f;
    private float dashForce = 0.0f;
    private float dashLimit = 1.0f;
    private float dashTime = 0.0f;

    //Input from the player
    private float movingDistance = 0.0f;

    //Player Components
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    private Animator animator;

    //Check whether player is jumping
    [SerializeField] private bool isJump = false;

    //CheckPoint
    private Vector3 checkPoint;

    //UI
    public DashIndicator dashIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //Retrieved all necesary components from Player object
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        //Calculate the Force it need to jump at the height we set.
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb2d.gravityScale));
        //TODO - Derived a formula to calcualte the force necessary
        dashForce = dashDistance * 10;

        checkPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input from user, multiply with time delta and apply speed to calculate distance player should move during a frame 
        movingDistance = Input.GetAxis("Horizontal") * speed;
        //Move the player transformation by distance calculated
        rb2d.velocity = new Vector2(movingDistance,rb2d.velocity.y);

        //If player is not jumping and space key is press
        if (!isJump && Input.GetKeyDown(KeyCode.Space))
        {
            //Apply upward force to player object to jump
            rb2d.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            //Set the Flag to true
            isJump = true;
        }
        //If player is jumping and y velocity is less than 0 (it means player is falling)
        else if(isJump && rb2d.velocity.y <= 0)
        {
            //Visualize the ray in scene (FOR DEBUG PURPOSE ONLY)
            Debug.DrawLine(ground.position, ground.position + Vector3.down * 0.5f);
            //Shoot a ray (a line) downwards from ground position to see if player hits anything
            RaycastHit2D hit = Physics2D.Raycast(ground.position, Vector2.down, 0.5f);
            //If player hit something (collider != null) and it's has a tag Ground => Player hit the ground
            isJump = (hit.collider == null || hit.collider.tag != "Ground");
        }

        //Dash
        if (dashTime >= dashLimit)
        {
            dashIndicator.DashEnable();
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashIndicator.DashDisable();
                rb2d.AddForce(new Vector2(Mathf.Floor(Input.GetAxis("Horizontal") * dashForce), 0), ForceMode2D.Impulse);
                dashTime = 0.0f;
            }
        }
        dashTime += Time.deltaTime;

        //Call function to handle animation
        HandleAnimation();
    }

    //Animation
    void HandleAnimation()
    {
        //If Jumping is false we allow player to play runnig animation
        if (!isJump)
        {
            animator.SetBool("isFalling", false);

            //If player is moving
            if (movingDistance != 0)
            {
                //Play the running animation
                animator.SetBool("isRunning", true);

                //Flip the player sprite horizontally depends on where player is going left or right
                if (sprite.flipX && movingDistance > 0)
                {
                    sprite.flipX = false;
                }
                else if (!sprite.flipX && movingDistance < 0)
                {
                    sprite.flipX = true;
                }
            }
            //Player is in idle
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        //Player is jumping
        else
        {
            //If the y-velocity is greater than 0, player is jumping upwards, so play upwards jump animation
            if (rb2d.velocity.y > 0) {
                animator.SetBool("isJump", true);
            }
        }

        //If it's negative y-velocity, player is falling, so play falling animation
        //This is outside of the jump condition because player can fall without jumping (ie. Falling of the platform)
        if(rb2d.velocity.y < -4f)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFalling", true);
        }
    }

    public void SetCheckPoint(Vector3 position)
    {
        checkPoint = position;
    }
    void OnBecameInvisible()
    {
        rb2d.velocity = Vector2.zero;
        transform.position = checkPoint;
    }
}
