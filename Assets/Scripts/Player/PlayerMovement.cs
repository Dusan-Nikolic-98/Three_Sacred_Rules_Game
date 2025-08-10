using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;

    private Rigidbody2D rb;
    private int moveDirection;
    private bool jumpReq;
    //private int groundContacts = 0;
    //private bool isGrounded => groundContacts > 0;

    //za bolju skok detekciju
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.05f;
    public LayerMask groundLayer;
    private bool isGrounded => Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

    private PlayerAbillities abilities;

    void Start() { 
        rb = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbillities>();
    }

    private void Update()
    {
        //citanje inputa
        if (Input.GetKey(moveLeftKey)) moveDirection = -1;
        else if (Input.GetKey(moveRightKey)) moveDirection = 1;
        else moveDirection = 0;

        if (Input.GetKeyDown(jumpKey)) {
            jumpReq = true;
        }
    }

    private void FixedUpdate()
    {

        if(abilities != null && abilities.IsDashing)
        { //da se ne krece ako dashuje
            return;
        }

        Vector2 newVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocityY);
        rb.linearVelocity = newVelocity;

        //okretanje
        if (moveDirection != 0) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x)*(moveDirection > 0 ? 1 : -1);
            transform.localScale = scale;
        }

        //skok
        if (jumpReq && isGrounded) {
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //isGrounded = false;
        }
        jumpReq = false;
    }

}