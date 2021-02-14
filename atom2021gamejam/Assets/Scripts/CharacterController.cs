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


    public float evolutionPoint = 0.1f;
    public Slider evolutionBar;

    public Animator animatorController;
    public bool facingRight = true;

    CapsuleCollider2D colliderElephant;
    CircleCollider2D birdCollider;
    BoxCollider2D monkeyCollider;



    public Transform banana;
    public Transform muzzle;


    public Slider canFlySlider;
    void Start()
    {
        Instance = this;
        animatorController = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }
        else
        {
            
        }


        if (currentCharacter == "Monkey")
        {

            if (Input.GetMouseButtonDown(0))
                ThrowBanana(50f);

            Move();
            Jump();
            BetterJump();
            CheckIfGrounded();

            renderer.sprite = characters[0].sprite;
            animatorController.runtimeAnimatorController = characters[0].animator;

            speed = 5.5f;

            if (!GetComponent<BoxCollider2D>())
            {
                monkeyCollider = gameObject.AddComponent<BoxCollider2D>();
                monkeyCollider.offset = new Vector2(0f, 0f);
                monkeyCollider.size = new Vector2(0.16f, 0.65f);
            }
            else
            {
                monkeyCollider.offset = new Vector2(0f, 0f);
                monkeyCollider.size = new Vector2(0.16f, 0.65f);
            }
        }
        else
        {
            if (GetComponent<BoxCollider2D>())
            {
                monkeyCollider.offset = new Vector2(0f, 0f);
                monkeyCollider.size = new Vector2(0f, 0f);
            }
        }

        if (currentCharacter == "Elephant")
        {
            Move();
            CheckIfGrounded();
            renderer.sprite = characters[1].sprite;
            animatorController.runtimeAnimatorController = characters[1].animator;
            speed = 2.5f;

            if (!GetComponent<CapsuleCollider2D>())
            {
                colliderElephant = gameObject.AddComponent<CapsuleCollider2D>();
                colliderElephant.direction = CapsuleDirection2D.Horizontal;
                colliderElephant.offset = new Vector2(0f, -0.12f);
                colliderElephant.size = new Vector2(0.45f, 0.25f);
            }
            else
            {
                colliderElephant.offset = new Vector2(0f, -0.12f);
                colliderElephant.size = new Vector2(0.45f, 0.25f);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                animatorController.SetBool("break", true);
            }
            else
            {
                animatorController.SetBool("break", false);
            }
        }
        else
        {
            if (GetComponent<CapsuleCollider2D>())
            {
                colliderElephant.offset = new Vector2(0f, 0f);
                colliderElephant.size = new Vector2(0f, 0f);
            }
        }

        if(currentCharacter == "Bird")
        {
            canFlySlider.gameObject.SetActive(true);

            BirdMove();
            CheckIfGrounded();
            renderer.sprite = characters[2].sprite;
            animatorController.runtimeAnimatorController = characters[2].animator;
            rigidbody2D.gravityScale = 10f;
            speed = 4f;


            if (!GetComponent<CircleCollider2D>())
            {
                birdCollider = gameObject.AddComponent<CircleCollider2D>();
                birdCollider.radius = 0.25f;
            }

            if (isGrounded)
            {
                animatorController.SetBool("idle", true);
                float xVelocity = rigidbody2D.velocity.x;
                xVelocity = 0f;
                rigidbody2D.velocity = new Vector2(xVelocity, vertical * speed);
                canFlySlider.value += 0.1f * Time.deltaTime;
            }
            else
            {
                BirdMove();
                animatorController.SetBool("idle", false);
                canFlySlider.value -= 0.1f * Time.deltaTime;
            }

            if (canFlySlider.value == 0)
            {
                float yVelocity = rigidbody2D.velocity.y;
                yVelocity = 0f;
                rigidbody2D.velocity = new Vector2(yVelocity, horizontal * speed);
            }
        }
        else
        {
            canFlySlider.gameObject.SetActive(false);

            rigidbody2D.gravityScale = 1;

            if (GetComponent<CircleCollider2D>())
            {
                birdCollider.radius = 0f;
            }
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

    void ThrowBanana(float throwSpeed)
    {
        Transform banan;
        banan = Instantiate(banana, muzzle.position, Quaternion.identity);
        banan.GetComponent<Rigidbody2D>().AddForce(muzzle.transform.forward * throwSpeed);
        banan.transform.Rotate(Vector3.one * 4 * Time.deltaTime);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rigidbody2D.velocity = new Vector2(moveBy, rigidbody2D.velocity.y);


        if (currentCharacter == "Elephant")
        {
            if (x != 0 && !animatorController.GetCurrentAnimatorStateInfo(0).IsName("break"))
            {
                animatorController.SetBool("walk", true);
            }
            else
            {
                animatorController.SetBool("walk", false);
            }
        }

        if (currentCharacter == "Monkey")
        {
            if (x != 0)
            {
                animatorController.SetBool("walk", true);
            }
            else
            {
                animatorController.SetBool("walk", false);
            }

            /*if (x != 0 && !animatorController.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                animatorController.SetBool("walk", true);
            }
            else
            {
                animatorController.SetBool("walk", false);
            }*/
        }
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
            animatorController.SetBool("jump", true);
        }
        else
        {
            animatorController.SetBool("jump", false);
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

    int breakCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EvolutionPoint")
        {
            evolutionBar.value += evolutionPoint;
            Destroy(collision.gameObject, 2f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentCharacter == "Elephant")
        {
            if (collision.tag == "Ground")
            {
                infoText.text = "Click 'S' button over and over!";
                if (Input.GetKeyDown(KeyCode.S))
                {
                    breakCount++;
                    if (breakCount == 3)
                    {
                        collision.gameObject.AddComponent<AddExplosionForGround>();
                        Destroy(collision.gameObject, 5f);
                        breakCount = 0;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        infoText.text = "";
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

