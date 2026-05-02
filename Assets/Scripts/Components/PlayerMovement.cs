using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Setup wajib untuk Top-Down 2D agar karakter tidak jatuh ke bawah layar
        rb.gravityScale = 0f; 
        rb.freezeRotation = true; // Biar karakter ga muter pas nabrak tembok
    }

    void Update()
    {
        // Cegah player gerak kalau state game bukan Gameplay (misal lagi buka overlay)
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Gameplay)
        {
            moveInput = Vector2.zero;
            return;
        }

        // Ambil input dari keyboard (WASD atau panah)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        // Normalize agar gerak diagonal tidak lebih cepat dari gerak lurus
        moveInput.Normalize(); 
    }

    void FixedUpdate()
    {
        // Eksekusi pergerakan di dalam FixedUpdate untuk fisika yang mulus
        rb.velocity = moveInput * moveSpeed;
        return;
    }
}