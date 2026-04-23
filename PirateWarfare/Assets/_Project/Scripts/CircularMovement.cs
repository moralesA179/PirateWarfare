using UnityEngine;
using UnityEngine.AI;

public class CircularMovement : MonoBehaviour
{
    public Transform player;
    public Cannon shipCannon;

    [Header("Distance Zones")]
    public float farRange = 10f;   // Outside this = chase
    public float shortRange = 5f;  // Inside this = stop and aim

    [Header("Speeds")]
    public float orbitSpeed = 1f;
    public float rotationSpeed = 360f;

    [Header("Sprite Rotation Offsets")]
    public float forwardOffset = 90f;    // Offset to make the front face the player
    public float broadsideOffset = 0f;   // Offset to make the side face the player (try 0f or 180f depending on your sprite)

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 1. SHORT RANGE: Stand still, broadside (sideways) to player
        if (distance <= shortRange)
        {
            agent.isStopped = true;
            RotateTowards(player.position, broadsideOffset);
        }
        // 2. MID RANGE: Revolve around the player, broadside (sideways) to player
        else if (distance <= farRange)
        {
            agent.isStopped = false;

            Vector2 dir = transform.position - player.position;
            float currentAngle = Mathf.Atan2(dir.y, dir.x);

            float targetAngle = currentAngle + orbitSpeed;

            float x = Mathf.Cos(targetAngle) * distance;
            float y = Mathf.Sin(targetAngle) * distance;
            Vector3 orbitPos = new Vector3(player.position.x + x, player.position.y + y, transform.position.z);

            agent.SetDestination(orbitPos);
            RotateTowards(player.position, broadsideOffset);
        }
        // 3. FAR RANGE: Sail straight towards the player (front facing)
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            RotateTowards(player.position, forwardOffset);
        }
    }

    void RotateTowards(Vector3 target, float angleOffset)
    {
        Vector2 direction = target - transform.position;
        // Replaced the buggy '+ rotationSpeed' with the dynamic angleOffset
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
    }
}