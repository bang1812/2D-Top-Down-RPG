using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0,399)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBurst;
    [SerializeField] private float restTime = 1;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger must be enable for oscillate to function properly.")]
    [SerializeField] private bool oscillate;

    private bool isShooting = false;
    private void OnValidate()
    {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectilesPerBurst < 1) {  projectilesPerBurst = 1; }
        if (burstCount < 1) {  burstCount = 1; }
        if (timeBetweenBurst < 0.1f) { timeBetweenBurst = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }
    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }
    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0;
        TargetConeOfInFluence(out startAngle, out currentAngle, out angleStep, out endAngle);
        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBurst / projectilesPerBurst;
        }
        for(int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                TargetConeOfInFluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInFluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;
                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }
                currentAngle += angleStep;
                if (stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }
            currentAngle = startAngle;
            if (!stagger)
            {
                yield return new WaitForSeconds(timeBetweenBurst);
            }

        }
        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInFluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDir = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0;
        angleStep = 0;
        if(angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }
    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }
}
