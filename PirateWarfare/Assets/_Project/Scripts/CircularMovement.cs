using UnityEngine;
using UnityEngine.AI;

public class CircularMovement : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 10f;     // distance from player
    public float orbitSpeed = 1f;       // how fast it circles

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

        agent.SetDestination(orbitPosition);

        // Optional: face the player while circling (like ships do 👀)
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
    }
}