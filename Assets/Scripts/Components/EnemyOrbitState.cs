using UnityEngine;

public class EnemyOrbitState : EnemyState
{
    private EnemyFlightController flightEnemy;
    private int orbitDirection; 
    private float randomSeed; 

    public EnemyOrbitState(EnemyFlightController enemyController) : base(null) 
    { 
        this.flightEnemy = enemyController;
    }

    public override void EnterState()
    {
        orbitDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        randomSeed = Random.Range(0f, 100f); 
    }

    public override void UpdateState()
    {
        if (flightEnemy.playerZeppelin == null) return;

        float noise = Mathf.PerlinNoise(Time.time * flightEnemy.wobbleSpeed, randomSeed) * 2f - 1f;
        float currentOrbitDistance = flightEnemy.orbitDistance + (noise * flightEnemy.wobbleAmount);

        float distanceToTarget = Vector2.Distance(flightEnemy.transform.position, flightEnemy.playerZeppelin.position);
        Vector2 directionToTarget = (flightEnemy.playerZeppelin.position - flightEnemy.transform.position).normalized;

        Vector2 targetVelocity;

        if (distanceToTarget > currentOrbitDistance)
        {
            Vector2 noisyApproach = directionToTarget + (Vector2.Perpendicular(directionToTarget) * noise * 0.3f);
            targetVelocity = noisyApproach.normalized * flightEnemy.flightSpeed;
        }
        else
        {
            Vector2 orbitDirectionVector = new Vector2(-directionToTarget.y * orbitDirection, directionToTarget.x * orbitDirection);
            Vector2 noisyOrbit = orbitDirectionVector + (directionToTarget * noise * 0.5f);
            targetVelocity = noisyOrbit.normalized * flightEnemy.flightSpeed;
        }

        Vector2 currentDir = flightEnemy.rb.velocity.normalized;
        Vector2 targetDir = targetVelocity.normalized;
        
        if (currentDir == Vector2.zero)
        {
            currentDir = targetDir;
        }

        Vector2 newDir = Vector3.RotateTowards(currentDir, targetDir, flightEnemy.turnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
        flightEnemy.rb.velocity = newDir * flightEnemy.flightSpeed;

        if (flightEnemy.rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(-flightEnemy.rb.velocity.y, flightEnemy.rb.velocity.x) * Mathf.Rad2Deg;
            
            if (angle < 0) angle += 360f;
            
            if(flightEnemy.transitionAnimation != null) 
            {
                flightEnemy.transitionAnimation.SetFloat("angle", angle);
            }
        }

        if (Time.time >= flightEnemy.nextFireTime && distanceToTarget <= flightEnemy.attackRange)
        {
            FireProjectile();
            flightEnemy.nextFireTime = Time.time + flightEnemy.fireRate; 
        }
    }

    private void FireProjectile()
    {
        if (flightEnemy.projectilePrefab != null && flightEnemy.firePoint != null)
        {
            Transform targetToShoot = GetClosestShipPart();

            if (targetToShoot == null)
            {
                targetToShoot = flightEnemy.playerZeppelin;
            }

            Vector2 directionToTarget = (targetToShoot.position - flightEnemy.firePoint.position).normalized;
            
            float bulletAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, bulletAngle);

            GameObject.Instantiate(flightEnemy.projectilePrefab, flightEnemy.firePoint.position, bulletRotation);
        }
    }

    public override void ExitState()
    {
    }

    private void RotateTowards(Vector2 targetDirection)
    {
        if (targetDirection == Vector2.zero) return;

        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        flightEnemy.transform.rotation = Quaternion.RotateTowards(flightEnemy.transform.rotation, targetRotation, flightEnemy.turnSpeed * Time.deltaTime);
    }

    private Transform GetClosestShipPart()
    {
        if (TargetablePart.activeParts.Count == 0) return null;

        Transform closestPart = null;
        float minDistanceSqr = Mathf.Infinity; 
        Vector3 currentPosition = flightEnemy.transform.position;

        foreach (TargetablePart part in TargetablePart.activeParts)
        {
            Vector3 directionToPart = part.transform.position - currentPosition;
            float dSqrToTarget = directionToPart.sqrMagnitude;

            if (dSqrToTarget < minDistanceSqr)
            {
                minDistanceSqr = dSqrToTarget;
                closestPart = part.transform;
            }
        }

        return closestPart;
    }
}

