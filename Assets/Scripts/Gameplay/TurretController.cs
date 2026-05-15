using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(LineRenderer))]
public class TurretController : MonoBehaviour
{
    [Header("Turret Parts")]
    [Tooltip("Bagian meriam yang akan berputar mengikuti mouse")]
    public Transform cannonBarrel; 
    [Tooltip("Titik peluru akan keluar")]
    public Transform firePoint; 

    [Header("Aiming Constraint (Cone)")]
    [Tooltip("Total sudut pandang tembakan (misal 90 derajat = 45 ke kiri, 45 ke kanan)")]
    public float coneAngle = 90f; 
    private float baseRotationAngle;

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    public float defaultZoom = 5f;
    public float shootingZoom = 8f;

    [Header("Combat Settings")]
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public float trajectoryRange = 15f;
    private float nextFireTime = 0f;

    private LineRenderer trajectoryLine;
    private bool isPlayerInRange = false;
    private bool isManningTurret = false;
    private GameObject player;

    void Start()
    {
        trajectoryLine = GetComponent<LineRenderer>();
        trajectoryLine.enabled = false;

        if (cannonBarrel != null)
        {
            baseRotationAngle = cannonBarrel.eulerAngles.z;
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ToggleTurretMode();
        }

        if (isManningTurret)
        {
            AimTurret();
            DrawTrajectory();

            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
            }
        }
    }

    private void ToggleTurretMode()
    {
        isManningTurret = !isManningTurret;

        if (isManningTurret)
        {
            virtualCamera.m_Lens.OrthographicSize = shootingZoom;
            trajectoryLine.enabled = true;
        
            player.GetComponent<PlayerMovement>().enabled = false; 
            
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize = defaultZoom;
            trajectoryLine.enabled = false;

            player.GetComponent<PlayerMovement>().enabled = true; 
        }
    }

    private void AimTurret()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = mouseWorldPos - cannonBarrel.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; 

        float angleDifference = Mathf.DeltaAngle(baseRotationAngle, targetAngle);
        

        float clampedDifference = Mathf.Clamp(angleDifference, -coneAngle / 2f, coneAngle / 2f);

        cannonBarrel.rotation = Quaternion.Euler(0, 0, baseRotationAngle + clampedDifference);
    }

    private void DrawTrajectory()
    {
        if (firePoint != null)
        {
            trajectoryLine.SetPosition(0, firePoint.position);
            
            trajectoryLine.SetPosition(1, firePoint.position + firePoint.up * trajectoryRange);
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = collision.gameObject;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isManningTurret) ToggleTurretMode(); 
            
            isPlayerInRange = false;
            player = null;
        }
    }
}