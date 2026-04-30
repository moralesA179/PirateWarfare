//using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Bullet to shoot
    public BasicProjectile projectile;
    public bool reloading = false;
    private bool burstDone = true;
    public float projectileSpeed = 4.0f;
    public bool enemy = false; //used to determine what type of projectile to shoot (friendly or not)

    public bool boss = false;
    public float bossScaleMultiplier = 2f; // multiplier for boss projectile size

    public enum CannonTypes { Base, Shotgun, Burst };
    public CannonTypes type;
    [Range(3, 10)]
    [Tooltip("Number of projectiles per shot")]
    public int maxProjectileCount = 3;

    [Header("Temporary Projectile Boost")]
    public int bonusProjectileCount = 0;
    public float projectileBoostTimer = 0f;

    public float reloadTimeInSeconds = 1.5f;
    private float currentReloadTimer = 0f;  

    private void Update()
    {
        if (reloading && burstDone)
        {
            currentReloadTimer += Time.deltaTime;

            if (currentReloadTimer >= reloadTimeInSeconds)
            {
                reloading = false;
                currentReloadTimer = 0f;
            }
        }
        else if (!reloading)
        {
            currentReloadTimer = 0f;
        }

        if (projectileBoostTimer > 0f)
        {
            projectileBoostTimer -= Time.deltaTime;

            if (projectileBoostTimer <= 0f)
            {
                projectileBoostTimer = 0f;
                bonusProjectileCount = 0;
            }
        }
    }

    public void SetCannonType(int type)
    {
        this.type = (CannonTypes)type;
    }

    public int GetCannonTypeInt()
    {
        return (int)this.type;
    }

    public static string GetCannonTypeStr(int type)
    {
        switch (type)
        {
            case 0:
                return "Base";
            case 1:
                return "Shotgun";
            case 2:
                return "Burst";
            default:
                return "";
        }
    }

    public int GetCurrentProjectileCount()
    {
        return maxProjectileCount + bonusProjectileCount;
    }

    public void ApplyProjectileBoost(int extraProjectiles, float duration)
    {
        bonusProjectileCount = extraProjectiles;
        projectileBoostTimer = duration;
    }

    public bool Shoot()
    {
        BasicProjectile cannonBall;
        Rigidbody2D rb;
        Collider2D shipCollider = GetComponentInParent<Collider2D>(); 

        if (projectile != null && !reloading && burstDone)
        {
            switch (type)
            {
                case CannonTypes.Base:
                    cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                    if (boss) cannonBall.transform.localScale *= bossScaleMultiplier; // Apply boss scale

                    if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                    cannonBall.enemy = enemy;
                    rb = cannonBall.GetComponent<Rigidbody2D>();
                    rb.linearVelocity = transform.right * projectileSpeed;
                    reloading = true;
                    return true;
                case CannonTypes.Shotgun:
                    ShotgunShot();
                    reloading = true;
                    return true;
                case CannonTypes.Burst:
                    StartCoroutine(BurstShot());
                    return true;
                default:
                    break;
            }
        }

        return false;

        IEnumerator BurstShot()
        {
            reloading = true;
            burstDone = false; // countdown doesn't start while we are still burst firing

            int projectileCount = GetCurrentProjectileCount();

            for (int i = 0; i < projectileCount; i++) 
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                if (boss) cannonBall.transform.localScale *= bossScaleMultiplier; // Apply boss scale

                if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                cannonBall.enemy = enemy;
                rb = cannonBall.GetComponent<Rigidbody2D>();
                rb.linearVelocity = transform.right * projectileSpeed;
                yield return new WaitForSeconds(0.1f);
            }
            burstDone = true;
        }

        void ShotgunShot()
        {
            int projectileCount = GetCurrentProjectileCount();

            if (projectileCount <= 1)
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                if (boss) cannonBall.transform.localScale *= bossScaleMultiplier; // Apply boss scale

                if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                cannonBall.enemy = enemy;
                rb = cannonBall.GetComponent<Rigidbody2D>();
                rb.linearVelocity = transform.right * projectileSpeed;
                return;
            }

            int remainder = projectileCount - 1; 
            float angleStep = 90f / remainder; // 45 for 2; for even bullet counts this angle step leads to there not being a straight shot bullet as it skips over 0 degs
            for (float i = -45f; i <= 45; i += angleStep)
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                if (boss) cannonBall.transform.localScale *= bossScaleMultiplier; // Apply boss scale

                if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                cannonBall.enemy = enemy;
                rb = cannonBall.GetComponent<Rigidbody2D>();
                Vector3 rotatedVector = Quaternion.AngleAxis(i, transform.forward.normalized) * transform.right; //rotating 30 degrees around z axis
                rb.linearVelocity = rotatedVector * projectileSpeed;
            }
        }
    }
}