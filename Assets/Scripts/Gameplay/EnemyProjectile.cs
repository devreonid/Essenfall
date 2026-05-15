using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = transform.up * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShipPart"))
        {
            Debug.Log("Peluru mengenai bagian kapal!");
            PartIntegrity part = collision.GetComponent<PartIntegrity>();
            if (part != null)
            {
                Debug.Log("Peluru mengenai target! Damage: " + damage);
                part.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }
    }
}