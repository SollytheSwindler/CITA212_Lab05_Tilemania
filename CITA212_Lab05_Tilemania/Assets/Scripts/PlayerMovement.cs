using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 20f);

    Vector2 moveInput;
    Rigidbody2D rigidbody;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D feetCollider;
    Animator animator;
    SpriteRenderer spriteRenderer;

    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        gravityScaleAtStart = rigidbody.gravityScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isAlive) return;
        spriteRenderer.color = Color.white;
        Run();
        ClimbLadder();
        FlipSprite();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms"))) return;

        if (value.isPressed)
        {
           rigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidbody.linearVelocity.y);
        rigidbody.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(rigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidbody.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(rigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        rigidbody.linearVelocity = climbVelocity;
        rigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rigidbody.linearVelocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            isAlive = false;
            spriteRenderer.color = Color.red;
            animator.SetTrigger("Dying");
            rigidbody.linearVelocity = deathKick;
        }
    }
}
