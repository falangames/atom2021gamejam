using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
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



    public GameObject beginPanel;

    public GameObject monkeyButton;
    public GameObject birdButton;
    public GameObject elephantButton;


    public Text beginText;

    public AudioSource throwBananaSound;

    void Start()
    {
        Instance = this;
        animatorController = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ClearInfoTexts());
    }

    bool messageShowElephant = false; 
    bool messageShowBird = false;

    void Update()
    {
        if (evolutionBar.value >= 0.2f)
        {
            monkeyButton.SetActive(true);
            elephantButton.SetActive(true);

            if (!messageShowElephant)
            {
                beginPanel.SetActive(true);
                beginText.text = "Now i can evolve to an elephant.(TAB)";
                //infoText.text = "Now i can evolve to an elephant.(TAB)";
                messageShowElephant = true;
            }
        }
        
        if (evolutionBar.value >= 0.4f)
        {
            birdButton.SetActive(true);
            if (!messageShowBird)
            {
                beginPanel.SetActive(true);
                beginText.text = "Now i can evolve to a bird.(TAB)";
                //infoText.text = "Now i can evolve to a bird.(TAB)";
                messageShowBird = true;
            }
        }

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
            {
                ThrowBanana(50f);
                throwBananaSound.mute = false;
                throwBananaSound.Play();
            }

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

            if (Input.GetKey(KeyCode.S))
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
        
        Destroy(banan.gameObject, 3);
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
            /*if (x != 0)
            {
                animatorController.SetBool("walk", true);
            }
            else
            {
                animatorController.SetBool("walk", false);
            }*/

            if (x != 0 && !animatorController.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                animatorController.SetBool("walk", true);
            }
            else
            {
                animatorController.SetBool("walk", false);
            }
        }
    }
    void BirdMove()
    {
        if (canFlySlider.value != 0)
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            vertical = 0;
            horizontal = 0;
        }

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
            Destroy(collision.gameObject, 1f);
        }

        if (collision.tag == "Died")
        {
            SceneManager.LoadScene("MainScene");
        }

        if (collision.tag == "Over")
        {
            StartCoroutine(OverGame());
        }
    }

    IEnumerator OverGame()
    {
        infoText.text = "Congratulations, you finished the game. Please wait 5 seconds.";
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("InfoScene");
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Over")
        {
            StartCoroutine(OverGame());
        }

        if (currentCharacter == "Elephant")
        {
            if (collision.tag == "Ground")
            {
                infoText.text = "Click 'S' button over and over!";
                if (Input.GetKey(KeyCode.S))
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
        else
        {
            infoText.text = "";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        infoText.text = "";
    }

    public void OnClick_BeginPanelExit()
    {
        beginPanel.SetActive(false);
    }

    IEnumerator ClearInfoTexts()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            infoText.text = "";
        }
    }
}
