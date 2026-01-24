using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;
    private float moveX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Cek apakah Rigidbody2D ada
        if (rb == null)
        {
            Debug.LogError("PlayerMovement butuh Rigidbody2D! Tambahkan Rigidbody2D ke object ini.");
            return;
        }
        
        rb.freezeRotation = true;
        
        // Memastikan deteksi tabrakan lebih akurat untuk objek yang bergerak cepat
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Menghaluskan pergerakan sprite
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (rb == null) return;

        // Ambil input di Update agar responsif
        moveX = Input.GetAxisRaw("Horizontal");

        // Berbalik arah sesuai gerakan (sprite default menghadap kiri)
        if (moveX > 0)
        {
            spriteRenderer.flipX = true; // Menghadap kanan
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = false; // Menghadap kiri
        }

        // Loncat (Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Reset velocity Y sebelum loncat agar kekuatan loncatan konsisten
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            Debug.Log("LONCAT!");
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Terapkan pergerakan di FixedUpdate untuk kestabilan fisika
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Mengecek apakah tabrakan terjadi dari bawah (tanah)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f) // Normal ke atas berarti kita di atas sesuatu
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
