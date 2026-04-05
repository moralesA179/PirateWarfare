using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Bullet to shoot
    public GameObject projectile;
    private bool reloading = false;

    //timer to prevent spam
    private int currentDelay = 0;
    [Tooltip("Number of FRAMES the reload should take to complete.")]
    public int targetDelay = 320;

    private void Update()
    {
        if (reloading)
        {
            currentDelay++;
            Debug.Log(currentDelay);
            reloading = currentDelay < targetDelay;
        }
        else
        {
            currentDelay = 0;
        }
    }
    public void Shoot()
    {
        if(projectile != null && !reloading) //if bullet exists and your arent currently reloading
        {
            GameObject cannonBall = Instantiate(projectile, transform.position , Quaternion.identity);
            Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * 3f;
            reloading = true;
        }
    }
    
}
