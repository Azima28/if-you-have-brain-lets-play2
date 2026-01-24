using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Cek apakah Rigidbody2D ada
        if (rb == null)
        {
            Debug.LogError("PlayerMovement butuh Rigidbody2D! Tambahkan Rigidbody2D ke object ini.");
            return;
        }
        
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (rb == null) return;

        // Gerak kiri-kanan (A/D)
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        // Loncat (Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            Debug.Log("LONCAT!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        Debug.Log("Menyentuh: " + collision.gameObject.name);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
