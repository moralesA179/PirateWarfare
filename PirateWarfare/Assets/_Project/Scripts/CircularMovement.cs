using UnityEngine;
using UnityEngine.AI;

public class CircularMovement : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 10f;      // Distance until shooting
    public float buffer = 2f;          
    public bool canShoot = false;

    // Regular enemy stats
    public float normalMoveSpeed = 3.5f;   
    public float normalOrbitSpeed = 0.5f;  
    public float normalRotationSpeed = 90f;

    // Boss stats
    public bool isBoss = false;
    public float bossMoveSpeed = 1.5f;      
    public float bossOrbitSpeed = 0.2f;    
    public float bossRotationSpeed = 15f;  

    // Sideways to player so cannon aims at player
    public float bowOffset = 90f;
    public float broadsideOffset = 0f;

    private bool isBuffered = false;
    private NavMeshAgent agent;
    private float currentOrbitAngle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player == null) return;

        agent.speed = isBoss ? bossMoveSpeed : normalMoveSpeed;
        float activeOrbitSpeed = isBoss ? bossOrbitSpeed : normalOrbitSpeed;

        float playerDistance = Vector2.Distance(transform.position, player.position);

        if (isBuffered && playerDistance > orbitRadius + buffer)
        {
            isBuffered = false;
        }
        else if (!isBuffered && playerDistance <= orbitRadius)
        {
            isBuffered = true;
            Vector2 dirToPlayer = transform.position - player.position;
            currentOrbitAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x);
        }

        if (!isBuffered)
        {
            // Chase state
            canShoot = false;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            RotateShip(false); // False = Bow forward
        }
        else
        {
            // Buffered prevents the enemy to switching states too fast 
            canShoot = true;

            if (playerDistance >= orbitRadius - buffer)
            {
                // Revolve around player
                agent.isStopped = false;

                currentOrbitAngle += activeOrbitSpeed * Time.deltaTime;

                float x = Mathf.Cos(currentOrbitAngle) * orbitRadius;
                float y = Mathf.Sin(currentOrbitAngle) * orbitRadius;

                Vector3 orbitPosition = new Vector3(
                    player.position.x + x,
                    player.position.y + y,
                    player.position.z
                );

                agent.SetDestination(orbitPosition);
                RotateShip(true); // True = Broadside
            }
            else
            {
                // If too close just rotate towards the enemy
                agent.isStopped = true;
                RotateShip(true); 

                Vector2 dirToPlayer = transform.position - player.position;
                currentOrbitAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x);
            }
        }
    }

    // Rotate towards the player
    private void RotateShip(bool isBroadside)
    {
        Vector2 direction = player.position - transform.position;
        float targetAngleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float finalTargetAngle = targetAngleToPlayer + (isBroadside ? broadsideOffset : bowOffset);

        float activeRotationSpeed = isBoss ? bossRotationSpeed : normalRotationSpeed;

        float currentAngle = transform.eulerAngles.z;
        float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, finalTargetAngle, activeRotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }
}