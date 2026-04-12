//using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Bullet to shoot
    public GameObject projectile;
    public bool reloading = false;
    private bool burstDone = true;
    public float projectileSpeed = 4.0f;
    public enum CannonTypes {Base, Shotgun, Burst};
    public CannonTypes type;
    [Range(3, 10)]
    [Tooltip("Number of projectiles per shot")]
    public int maxProjectileCount = 3; 

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
    }
    public void Shoot()
    {
        GameObject cannonBall; 
        Rigidbody2D rb;
        if (projectile != null && !reloading && burstDone) //if bullet exists and your arent currently reloading
        {
            switch (type)
            {
                case CannonTypes.Base:
                    cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
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
            for (int i = 0; i < maxProjectileCount; i++)
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                rb = cannonBall.GetComponent<Rigidbody2D>();
                rb.linearVelocity = transform.right * projectileSpeed;
                yield return new WaitForSeconds(0.1f);
            }
            burstDone = true;
        }

        void ShotgunShot()
        {
            int remainder = maxProjectileCount - 1; //number of bullets to be split on negative and positive quadrants (ex. 2 for 3 or 3 for 4) 
            float angleStep = 90 / remainder; // 45 for 2; for even bullet counts this angle step leads to there not being a straight shot bullet as it skips over 0 degs
            for(float i = -45f; i <= 45; i+= angleStep)
            {
                cannonBall = Instantiate(projectile, transform.position, Quaternion.identity);
                rb = cannonBall.GetComponent<Rigidbody2D>();
                Vector3 rotatedVector = Quaternion.AngleAxis(i, transform.forward.normalized) * transform.right; //rotating 30 degrees around z axis
                rb.linearVelocity = rotatedVector * projectileSpeed;
            }
        }
    }
    
}
