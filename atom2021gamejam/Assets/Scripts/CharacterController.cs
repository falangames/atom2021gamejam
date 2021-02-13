using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    /*[SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;


    private string currentCharacter = "Monkey";

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool grounded;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (grounded)
        {
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                // Calculate the velocity required to achieve the target jump height.
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }

        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);

        grounded = false;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                }
            }
        }*/

    public static CharacterController Instance;

    public float speed;
    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public GameObject CharacterPickPanel;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public Character[] characters;


    public bool isGrounded = false;
    public string currentCharacter = "Monkey";
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer renderer;
    public float horizontal;
    public float vertical;

    public Text infoText;
    void Start()
    {
        Instance = this;
        rigidbody2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (currentCharacter == "Monkey")
        {
            Move();
            Jump();
            BetterJump();
            CheckIfGrounded();
            renderer.sprite = characters[0].sprite;
            speed = 5.5f;
        }

        if (currentCharacter == "Elephant")
        {
            Move();
            CheckIfGrounded();
            renderer.sprite = characters[1].sprite;
            speed = 2.5f;
        }

        if(currentCharacter == "Bird")
        {
            BirdMove();
            CheckIfGrounded();
            renderer.sprite = characters[2].sprite;
            rigidbody2D.gravityScale = 10f;
            speed = 4f;
        }
        else
        {
            rigidbody2D.gravityScale = 1;
        }



        if (Input.GetKey(KeyCode.Tab) && isGrounded)
        {
            CharacterPickPanel.SetActive(true);
        }
        else
        {
            CharacterPickPanel.SetActive(false);
        }
    }


    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rigidbody2D.velocity = new Vector2(moveBy, rigidbody2D.velocity.y);
    }
    void BirdMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rigidbody2D.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
        }
    }

    void BetterJump()
    {
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

        if (colliders != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void OnClick_MonkeyButton()
    {
        currentCharacter = "Monkey";
    }
    public void OnClick_ElephantButton()
    {
        currentCharacter = "Elephant";
    }
    public void OnClick_BirdButton()
    {
        currentCharacter = "Bird";
    }

    int breakCount = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            if (currentCharacter == "Elephant")
            {
                infoText.text = "Click 'S' button over and over.";
                if (Input.GetKeyDown(KeyCode.S))
                {
                    breakCount++;
                    if (breakCount == 3)
                    {
                        collision.gameObject.AddComponent<AddExplosionForGround>();
                        Destroy(collision.gameObject, 5f);
                        breakCount = 0;
                        infoText.text = "";
                    }
                }
            }
            else
            {
                infoText.text = "";
            }
        }
    }
}
























/*public void OnClick_MonkeyButton()
{
    currentCharacter = "Monkey";
}
public void OnClick_ElephantButton()
{
    currentCharacter = "Elephant";
}
public void OnClick_BirdButton()
{
    currentCharacter = "Bird";
}*/

/*public float speed = 100.0f;
public float jumpForce = 350.0f;
public Transform bottomTransform;

private Rigidbody2D body;
//private Animator animator;
private SpriteRenderer spriteRenderer;

private Vector2 currentVelocity;
private float previousPositionY;

private bool isOnGround;


// Start is called before the first frame update
void Start()
{
    body = GetComponent<Rigidbody2D>();
    //animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
}

// Update is called once per frame
void Update()
{

}

private void FixedUpdate()
{
    Move();
    HandleCollisions();
    previousPositionY = transform.position.y;
}

private void Move()
{
    float velocity = Input.GetAxis("Horizontal") * speed;
    bool isJumping = Input.GetKey(KeyCode.Space);

    //animator.SetFloat("Speed", Mathf.Abs(velocity));
    // Horizontal Movement
    body.velocity = Vector2.SmoothDamp(body.velocity, new Vector2(velocity, body.velocity.y), ref currentVelocity, 0.02f);

    // Initiate Jump
    if (isOnGround && isJumping)
    {
        //animator.SetBool("IsJumping", true);
        isOnGround = false;
        body.AddForce(new Vector2(0, jumpForce));
    }

    // Cancel Jump
    if (!isOnGround && !isJumping && body.velocity.y > 0.01f)
    {
        body.velocity = new Vector2(body.velocity.x, body.velocity.y);
    }

    if (velocity < 0) spriteRenderer.flipX = true;
    else if (velocity > 0)
        spriteRenderer.flipX = false;
}

private void HandleCollisions()
{
    bool wasOnGround = isOnGround;
    isOnGround = false;

    Collider2D[] colliders = Physics2D.OverlapCircleAll(bottomTransform.position, 0.6f);
    foreach (var collider in colliders)
    {
        if (collider.gameObject != gameObject)
        {
            isOnGround = true;
            if (!wasOnGround && previousPositionY > transform.position.y)
                HandleLanding();
        }
    }
}

private void HandleLanding()
{
    //animator.SetBool("IsJumping", false);
}*/

