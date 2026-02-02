using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isGrounded = false;
    private float moveX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
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
        
        // PENTING: Cegah Rigidbody "tidur" agar ground detection selalu aktif
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Biar tidak nempel di tembok saat loncat (friction = 0)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            PhysicsMaterial2D noFriction = new PhysicsMaterial2D("NoFriction");
            noFriction.friction = 0f;
            noFriction.bounciness = 0f;
            col.sharedMaterial = noFriction;
        }
    }

    void Update()
    {
        if (rb == null) return;

<<<<<<< Updated upstream
        // Gabungkan input dari MoveButton (mobile) DAN keyboard
        float mobileInput = MoveButton.Input;
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        
        // Prioritas: mobile jika ada, kalau tidak pakai keyboard
        if (Mathf.Abs(mobileInput) > 0.1f)
        {
            moveX = mobileInput;
        }
        else
        {
            moveX = keyboardInput;
        }
=======
        // Jangan proses input jika game sedang di-pause
        if (Pause.IsPaused)
        {
            moveX = 0;
            return;
        }

        // Ambil input di Update agar responsif
        moveX = Input.GetAxisRaw("Horizontal");
>>>>>>> Stashed changes

        // Berbalik arah sesuai gerakan (sprite default menghadap kiri)
        if (moveX > 0)
        {
            spriteRenderer.flipX = true; // Menghadap kanan
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = false; // Menghadap kiri
        }

        // Set animator parameters
        if (animator != null)
        {
            bool isMoving = Mathf.Abs(moveX) > 0;
            
            animator.SetBool("IsIdle", !isMoving && isGrounded);
            animator.SetBool("IsRunning", isMoving && isGrounded);
            animator.SetBool("IsJump", !isGrounded);
        }

        // Loncat (Space keyboard ATAU JumpButton mobile)
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || JumpButton.IsPressed;
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Terapkan pergerakan di FixedUpdate untuk kestabilan fisika
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
    }

    private int groundContactCount = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Deteksi langsung saat mendarat
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                groundContactCount++;
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Tetap pastikan isGrounded benar jika masih ada kontak
        if (!isGrounded)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Hanya set false jika sudah tidak ada kontak dengan tanah sama sekali
        // Kita perlu mengecek apakah kontak yang keluar adalah kontak tanah
        // Namun cara termudah dan cukup efektif adalah dengan counter atau raycast.
        // Di sini saya gunakan pendekatan sederhana: cek ulang saat exit.
        
        // Sebagai alternatif yang lebih aman:
        StartCoroutine(CheckGroundedNextFrame());
    }

    private IEnumerator CheckGroundedNextFrame()
    {
        yield return new WaitForFixedUpdate();
        // Sederhana: kita anggap tidak grounded dulu, lalu biarkan Stay/Enter yang membenarkannya
        // Atau biarkan logic counter (jika diimplementasikan penuh).
        // Untuk sekarang, kita gunakan pendekatan yang lebih stabil:
        isGrounded = false;
    }
}
