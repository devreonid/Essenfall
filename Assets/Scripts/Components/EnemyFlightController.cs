using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFlightController : MonoBehaviour
{
    [Header("Health Stats")]
    public int maxHP = 50;
    [HideInInspector] public int currentHP;

    [Header("Flight Stats")]
    public float flightSpeed = 5f;
    public float turnSpeed = 200f; 
    public float orbitDistance = 5f; 

    [Header("Orbit Noise Settings")] 
    public float wobbleSpeed = 0.5f;
    public float wobbleAmount = 2.0f;
    public Animator transitionAnimation;

    [Header("Combat Stats")]
    public float attackRange = 7f; 
    public float fireRate = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [HideInInspector] public Transform playerZeppelin; 
    [HideInInspector] public float nextFireTime = 0f;

    private EnemyState currentState;
    public EnemyOrbitState OrbitState { get; private set; }

    [HideInInspector] public Rigidbody2D rb;

    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 

        impulseSource = GetComponent<CinemachineImpulseSource>();

        OrbitState = new EnemyOrbitState(this);
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerZeppelin");
        if (playerObj != null) playerZeppelin = playerObj.transform;
        Debug.Log(playerZeppelin != null ? "Player Zeppelin ditemukan!" : "Peringatan: Player Zeppelin tidak ditemukan! Pastikan ada objek dengan tag 'PlayerZeppelin' di scene.");
        
        ChangeState(OrbitState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (impulseSource == null)
        {
            Debug.LogWarning("CinemachineImpulseSource tidak ditemukan di " + gameObject.name + ". Pastikan komponen ini ditambahkan untuk efek getaran saat musuh hancur.");
        }
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(0.5f);
        }

        Destroy(gameObject);
    }
}