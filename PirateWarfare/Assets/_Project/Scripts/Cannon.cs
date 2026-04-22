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
    public enum CannonTypes { Base, Shotgun, Burst };
    public CannonTypes type;
    [Range(3, 10)]
    [Tooltip("Number of projectiles per shot")]
    public int maxProjectileCount = 3;

    [Header("Temporary Projectile Boost")]
    public int bonusProjectileCount = 0;
    public float projectileBoostTimer = 0f;

    //timer to prevent spam
    private int currentDelay = 0;
    [Tooltip("Number of FRAMES the reload should take to complete.")]
    public int targetDelay = 320;

    private void Update()
    {
        if (reloading && burstDone) //only start count when the burstfire is done 
        {
            currentDelay++;
            //Debug.Log(currentDelay);
            reloading = currentDelay < targetDelay;
        }
        else
        {
            currentDelay = 0;
        }

        //Debug.DrawLine(transform.position, transform.position + transform.right, Color.red);
        //Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);
        //Debug.Log($"({transform.right.normalized}, {transform.up.normalized})");
        //Debug.DrawLine(transform.position, transform.position + (transform.up + transform.right));
        //Debug.DrawLine(transform.position, transform.position + (-transform.up + transform.right));
        //Vector3 l = Quaternion.AngleAxis(30f, transform.forward.normalized) * transform.right;
        //Debug.DrawLine(transform.position, transform.position + l, Color.blue);

        //Count down temporary projectile boost
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

    public void Shoot()
    {
        BasicProjectile cannonBall;
        Rigidbody2D rb;
        Collider2D shipCollider = GetComponentInParent<Collider2D>(); // Added collider reference

        if (projectile != null && !reloading && burstDone) //if bullet exists and your arent currently reloading
        {
            switch (type)
            {
                case CannonTypes.Base:
                    cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                    if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                    cannonBall.enemy = enemy;
                    rb = cannonBall.GetComponent<Rigidbody2D>();
                    rb.linearVelocity = transform.right * projectileSpeed;
                    reloading = true;
                    break;
                case CannonTypes.Shotgun:
                    ShotgunShot();
                    reloading = true;
                    break;
                case CannonTypes.Burst:
                    StartCoroutine(BurstShot());
                    break;
                default:
                    break;
            }
        }

        IEnumerator BurstShot()
        {
            reloading = true;
            burstDone = false; //extra field to ensure countdown doesn't start while we are still burst firing!

            int projectileCount = GetCurrentProjectileCount();

            for (int i = 0; i < projectileCount; i++) // Fixed: Used projectileCount to allow powerups to work
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
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
                if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                cannonBall.enemy = enemy;
                rb = cannonBall.GetComponent<Rigidbody2D>();
                rb.linearVelocity = transform.right * projectileSpeed;
                return;
            }

            //int counter = 0; //debug
            int remainder = projectileCount - 1; // Fixed: Used projectileCount to allow powerups to work
            float angleStep = 90f / remainder; // 45 for 2; for even bullet counts this angle step leads to there not being a straight shot bullet as it skips over 0 degs
            for (float i = -45f; i <= 45; i += angleStep)
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                if (shipCollider != null) Physics2D.IgnoreCollision(cannonBall.GetComponent<Collider2D>(), shipCollider); // Ignored collision
                cannonBall.enemy = enemy;
                rb = cannonBall.GetComponent<Rigidbody2D>();
                Vector3 rotatedVector = Quaternion.AngleAxis(i, transform.forward.normalized) * transform.right; //rotating 30 degrees around z axis
                rb.linearVelocity = rotatedVector * projectileSpeed;
                //Debug.Log("Iteration: " + counter++);
            }
        }
    }

}
