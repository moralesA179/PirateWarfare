using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(100f, 1000f)]
    public float lifeTime;
    public enum ProjectileTypes { Base, Richochet };
    public ProjectileTypes type;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0)
            lifeTime--;
        else
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            switch(type)
            {
                case ProjectileTypes.Base:
                    lifeTime = 0; //immediately kill upon collision
                    break;
                case ProjectileTypes.Richochet:
                    rb.linearVelocity = -rb.linearVelocity;
                    break;
            }
        }
        else
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider); //need to do something else for this once enemies are added
        }
    }
}
