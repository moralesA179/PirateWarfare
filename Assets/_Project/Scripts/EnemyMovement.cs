using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // assign your Player in the Inspector
    private NavMeshAgent agent;
    public float rotationSpeed = 360.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    void Update()
    {
        agent.SetDestination(player.position);
        Vector3 direction = player.position - transform.position;

        if (direction.sqrMagnitude > 0.1f) // Only rotate if we aren't already on top of the player
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}