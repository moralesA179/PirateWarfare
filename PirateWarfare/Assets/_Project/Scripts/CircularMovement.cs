using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class CircularMovement : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 10f;     // distance from player
    public float orbitSpeed = .5f;       // how fast it circles
    public float rotationSpeed = 5.0f;  
    public float buffer = 2f;
    private bool isRevolving = false;
    private bool isBuffered = false;
    private float playerDistance;
    

    private NavMeshAgent agent;
    private float angle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player == null) return;

        playerDistance = Vector2.Distance(transform.position, player.position);

        //if (playerDistance > orbitRadius && !isBuffered)
        //{
            // 1. Calculate the direction vector
            Vector2 direction = player.position - transform.position;

            // 2. Find the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 3. Create the target rotation
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // 4. Smoothly rotate toward the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //}

        // Increase angle over time
        angle += orbitSpeed * Time.deltaTime;

        // Calculate circular position around player
        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

        Vector3 orbitPosition = new Vector3(
            player.position.x + x,
            player.position.y + y,
            player.position.z
        );

        agent.SetDestination(player.position);
    }
}