using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Animator")]
    public Animator animator;   // WAJIB di-assign di Inspector

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
<<<<<<< Updated upstream
    private Animator animator;
    private bool isGrounded = false;
=======

>>>>>>> Stashed changes
    private float moveX;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
<<<<<<< Updated upstream
        animator = GetComponent<Animator>();
        
        // Cek apakah Rigidbody2D ada
=======

>>>>>>> Stashed changes
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D TIDAK ditemukan!");
            enabled = false;
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator BELUM di-assign!");
            enabled = false;
            return;
        }

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        // Flip sprite (default menghadap kiri)
        if (moveX > 0)
            spriteRenderer.flipX = true;
        else if (moveX < 0)
            spriteRenderer.flipX = false;

<<<<<<< Updated upstream
        // Set animator parameters
        if (animator != null)
        {
            bool isMoving = Mathf.Abs(moveX) > 0;
            
            animator.SetBool("IsIdle", !isMoving && isGrounded);
            animator.SetBool("IsRunning", isMoving && isGrounded);
            animator.SetBool("IsJump", !isGrounded);
        }

        // Loncat (Space)
=======
        // Animator
        animator.SetBool("isRunning", moveX != 0);

        // Jump
>>>>>>> Stashed changes
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D c in collision.contacts)
        {
            if (c.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
