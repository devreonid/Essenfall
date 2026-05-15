using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;

    private bool isFacingRight = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 0f; 
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Gameplay)
        {
            moveInput = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); 

        FlipCharacter();

        anim.SetFloat("Speed", moveInput.magnitude); 
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    private void FlipCharacter()
    {
        if (moveInput.x > 0 && !isFacingRight || moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = transform.localScale;
            
            localScale.x *= -1f;
            
            transform.localScale = localScale;
        }
    }
}