using UnityEngine;
using UnityEngine.UIElements;

public class BasicProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(100f, 1000f)]
    public float lifeTime;
    [Range(0.1f, 5f)]
    public float homingRadius = 0.5f;
    [Range(0.01f, 0.1f)]
    public float homingStrength = 0.02f;
    public enum ProjectileTypes { Base, Richochet, Homing};
    private float counter = 0.01f;
    public bool enemy = false;
    public ProjectileTypes type;
    Rigidbody2D rb;

    public MusicPlayer musicPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        musicPlayer = GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Lifetime: " + lifeTime);
        if (lifeTime > 0)
        {
            lifeTime--;
        }
        else   
        {
            Destroy(gameObject);
        }

        if (type == ProjectileTypes.Homing) //homing projectiles need to continously update their linear velocity to go in direction of the enemy...
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, homingRadius);
            if (hit != null)
            {
                if (hit.CompareTag("Enemy")) //if the enemy was in our detection radius
                {
                    //Debug.Log("Should home in...");
                    //rb.linearVelocity = Vector3.RotateTowards(rb.linearVelocity.normalized, (hit.transform.position - transform.position).normalized, 0.1f, 0.0f).normalized * 4f;
                    //Lerp more the farther you are from target...
                    rb.linearVelocity = 4f * Vector3.Lerp(rb.linearVelocity.normalized, (hit.transform.position - transform.position).normalized, counter * Time.deltaTime).normalized;
                    //Debug.DrawLine(transform.position, transform.position + (hit.transform.position - transform.position).normalized, Color.red);
                    //Debug.DrawLine(transform.position, transform.position + (Vector3)rb.linearVelocity.normalized, Color.green);
                    counter += homingStrength;
                }
            }
            //Debug.Log($"Linear Velocity Magnitude: {rb.linearVelocity.magnitude}");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Cannon ball hit something");
        // 1. Check if we need to ignore this collision entirely (Friendly fire check)
        if (!enemy && (collision.collider.CompareTag("Player") 
            || collision.collider.CompareTag("Projectile")) 
            || enemy && (collision.collider.CompareTag("Enemy")  
            || collision.collider.CompareTag("World")))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return; // Exit early so the bullet doesn't process damage or die
        }

        // 2. Handle Damage Logic
        if (enemy)
        {
            // Enemy projectile hitting the player
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("I collided with the player from enemy!");
                PlayerData.TakeDamage(10);
            }
        }
        else
        {
            Debug.Log("Not a player HIT!");
            // Friendly projectile hitting an enemy
            if (collision.collider.CompareTag("Enemy"))
            {
                Debug.Log("Friendly projectile hit an enemy!");
                collision.collider.GetComponent<Enemy>().TakeDamage(PlayerData.baseDamage);
            }
        }

        // 3. Handle Projectile Physical Behavior (Applies to both enemy and friendly)
        switch (type)
        {
            case ProjectileTypes.Base:
            case ProjectileTypes.Homing:
                lifeTime = 0; // immediately kill upon collision
                break;

            case ProjectileTypes.Richochet:
                // Reflect the velocity across the normal of the surface we just hit
                Vector2 incomingVelocity = rb.linearVelocity;
                Vector2 surfaceNormal = collision.contacts[0].normal;

                rb.linearVelocity = Vector2.Reflect(incomingVelocity.normalized, surfaceNormal) * incomingVelocity.magnitude;
                break;
        }
    }

    public string GetProjectileTypeStr()
    {
        switch(this.type)
        {
            case ProjectileTypes.Base:
                return "Base";
            case ProjectileTypes.Richochet:
                return "Richochet";
            case ProjectileTypes.Homing:
                return "Homing";
            default:
                return "";
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, homingRadius);
    }
}
