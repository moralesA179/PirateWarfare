using UnityEngine;
using UnityEngine.UIElements;

public class BasicProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(100f, 1000f)]
    public float lifeTime;
    public enum ProjectileTypes { Base, Richochet };

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
        if (lifeTime > 0)
        {
            lifeTime--;
        }
        else   
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) 
        {
            Debug.Log("collision detected");
            musicPlayer.Damage();
        }
            if (enemy)
        {
            if(collision.collider.CompareTag("Player"))
            {
                Debug.Log("I collided with the player from enemy!");
                switch(type)
                {
                    case ProjectileTypes.Base:
                        Destroy(gameObject, 1.056f); //immediately kill upon collision with player
                        break;
                    case ProjectileTypes.Richochet:
                        rb.linearVelocity = -rb.linearVelocity;
                        break;
                }
                PlayerData.TakeDamage(10);
            }
        }
        else
        {
            if (!collision.collider.CompareTag("Player"))
            {

                switch (type)
                {
                    case ProjectileTypes.Base:
                        Destroy(gameObject, 1.056f); //immediately kill upon collision
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
}
