using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;

    Rigidbody2D myrb;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    float gravityScaleAtStart;
    bool isAlive = true;

    void Start()
    {
        myrb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        Debug.Log("Hello World!");
        gravityScaleAtStart = myrb.gravityScale;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive) 
        { 
            return;
        }

        Instantiate (bullet, gun.position, transform.rotation);
    }
    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if(value.isPressed)
        {
            myrb.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myrb.linearVelocity.y);
        myrb.linearVelocity = playerVelocity;

        //whenever the x is positive the animation will change fron idle to running
        bool playerHasHorizontalSpeed = Mathf.Abs(myrb.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        //mathf.Abs means -10.5 = 10.5
        //mathf.Epsilon = 0
        bool playerHasHorizontalSpeed = Mathf.Abs(myrb.linearVelocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            //mathf.Sign means return 1 when x is positive, return -1 when x is negative
            transform.localScale = new Vector2(Mathf.Sign(myrb.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myrb.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2 (myrb.linearVelocity.x, moveInput.y * climbSpeed);
        myrb.linearVelocity = climbVelocity;
        myrb.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myrb.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")) || myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myrb.linearVelocity = deathKick;

            StartCoroutine(DelayDeathTime());
        }
    }

    IEnumerator DelayDeathTime()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
    