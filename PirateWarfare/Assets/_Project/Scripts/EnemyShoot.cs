using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    float fireRate = 1f;
    float nextFireTime = 0f;
    Rigidbody2D rb;
    public Transform t;

    Cannon[] cannons;
    //public Cannon leftCannon;
    //public Cannon rightCannon;

    CircularMovement circularMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cannons = GetComponentsInChildren<Cannon>();
        circularMovement = GetComponent<CircularMovement>();
        //Debug.Log("Cannons found: " + cannons.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime && circularMovement.canShoot)
        {
            Debug.Log("SHOT! , can shoot? " + circularMovement.canShoot);
            if (t.right.x > 0)
            {
                cannons[0].Shoot();
                //Debug.Log("Shoot 1");
            }
            else if (t.right.x < 0)
            {
                cannons[1].Shoot();
                //Debug.Log("Shoot 2");
            }

            nextFireTime = Time.time + fireRate;
        }
    }
}