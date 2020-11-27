using UnityEngine;

// This script manages the player's movement and have configurable variables

public class PlayerMovement : MonoBehaviour
{
    #region Singleton
    private static PlayerMovement instance;
    public static PlayerMovement GetInstance() { return instance; }

    void Awake()
    {
        // Check if there isn't another instance
        if (instance != null && instance != this)
        {
            // Destroy the duplicate
            Destroy(this.gameObject);
        }
        else
        {
            // There isn't an instance set this as the instance
            instance = this;
        }
    }
    #endregion

    // Variables
    public float moveSpeed = 7.5f;              // Determines player move speed
    public float jumpPower = 450f;              // Determines player base jump power

    public float fallMultiplier = 2.5f;         // Increases fall speed for normal jumps
    public float lowFallMultiplier = 2f;        // Increases fall speed for low jumps

    public bool isGrounded = true;              // Is the player on the ground?
    public bool canJump = true;                 // Can the player Jump?

    Rigidbody2D rb2d;                           // Reference to the rigidbody
    Animator animator;                          // Reference to the Animator

    public GameObject playersprite;

    void Start()
    {
        // Finds the rigidbody attached to the gameobjects child
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Stores local variables
        var moveX = Input.GetAxis("Horizontal");
        var jumpPressed = Input.GetButtonDown("Jump");

        // Move Character
        transform.position += new Vector3(moveX * moveSpeed * Time.deltaTime, 0f, 0f);

        // Do we press/hold the jump button and are we grounded...
        if (jumpPressed && isGrounded)
        {
            // ...jump
            rb2d.AddForce(new Vector2(0f, jumpPower));
        }

        // Set Set animation variable speed equal to moveX float
        float characterSpeed = moveX;
        animator.SetFloat("speed", characterSpeed);

        if (moveX <= -0.05)
        {
            animator.SetBool("isrunning", true);
            playersprite.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveX >= 0.05) {
            animator.SetBool("isrunning", true);
            playersprite.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            animator.SetBool("isrunning", false);
        }
   
    }

    // Because we use Unity Physics it's saver to do Physics calculations in the FixedUpdate
    void FixedUpdate()
    { 
        // Check if we are falling...
        if (rb2d.velocity.y < 0)
        {
            // ...yes, apply normal fall modifier
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // ...no, apply low fall modifier
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowFallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}