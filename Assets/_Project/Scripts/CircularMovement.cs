using UnityEngine;
using UnityEngine.AI;

public class CircularMovement : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 10f;      // Base distance to chase until
    public float buffer = 2f;            // Creates the 8m to 12m revolve zone
    public bool canShoot = false;

    [Header("Normal Ship Speeds")]
    public float normalMoveSpeed = 3.5f;    // How fast it chases (NavMesh speed)
    public float normalOrbitSpeed = 0.5f;   // How fast it circles
    public float normalRotationSpeed = 90f; // How fast it turns

    [Header("Boss Settings")]
    public bool isBoss = false;
    public float bossMoveSpeed = 1.5f;      // Bosses chase slower
    public float bossOrbitSpeed = 0.2f;     // Bosses revolve slower
    public float bossRotationSpeed = 15f;   // Bosses turn much slower

    [Header("Sprite Rotation Offsets")]
    [Tooltip("Adjust this if the ship doesn't face the player correctly while chasing.")]
    public float bowOffset = 90f;
    [Tooltip("Adjust this if the wrong side of the ship faces the player while shooting (Usually 90 degrees different from the bow).")]
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

        // Automatically set the correct speeds based on if this ship is a boss
        agent.speed = isBoss ? bossMoveSpeed : normalMoveSpeed;
        float activeOrbitSpeed = isBoss ? bossOrbitSpeed : normalOrbitSpeed;

        float playerDistance = Vector2.Distance(transform.position, player.position);

        // 1. Manage the Buffer State
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

        // 2. Execute Behaviors
        if (!isBuffered)
        {
            // CHASE STATE (Outside 12m) - Move straight, Bow forward
            canShoot = false;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            RotateShip(false); // False = Bow forward
        }
        else
        {
            // BUFFERED STATE 
            canShoot = true;

            if (playerDistance >= orbitRadius - buffer)
            {
                // REVOLVE STATE (Between 8m and 12m) - Move circular, Broadside
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
                // TOO CLOSE STATE (Under 8m) - Stop moving, keep cannons aimed
                agent.isStopped = true;
                RotateShip(true); // True = Broadside

                // Sync angle so we don't snap if they move away
                Vector2 dirToPlayer = transform.position - player.position;
                currentOrbitAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x);
            }
        }
    }

    // Handles the rotation based on combat state
    private void RotateShip(bool isBroadside)
    {
        Vector2 direction = player.position - transform.position;
        float targetAngleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the correct offset depending on if we are chasing or shooting
        float finalTargetAngle = targetAngleToPlayer + (isBroadside ? broadsideOffset : bowOffset);

        // Get the correct rotation speed
        float activeRotationSpeed = isBoss ? bossRotationSpeed : normalRotationSpeed;

        // Smoothly rotate towards the target angle
        float currentAngle = transform.eulerAngles.z;
        float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, finalTargetAngle, activeRotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }
}