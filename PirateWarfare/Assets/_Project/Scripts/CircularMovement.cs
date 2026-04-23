
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class CircularMovement : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 10f;     // distance from player
    public float orbitSpeed = .5f;       // how fast it circles
    public float rotationSpeed = 360.0f;
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

        Vector3 direction = player.position - transform.position;

        playerDistance = Vector2.Distance(transform.position, player.position);

        if (playerDistance > orbitRadius && !isBuffered)
        {
            agent.SetDestination(player.position);

            direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (playerDistance <= orbitRadius && !isBuffered)
        {
            isBuffered = true;
        }
        else if (isBuffered)
        {
            if (playerDistance > orbitRadius + buffer)
            {
                isBuffered = false;
                agent.isStopped = false;

            }

            else if (playerDistance > orbitRadius - buffer && playerDistance < orbitRadius + buffer)
            {
                agent.isStopped = false;

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

            else if (playerDistance < orbitRadius - buffer)
            {
                agent.isStopped = true;
                direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

        }
    }
}