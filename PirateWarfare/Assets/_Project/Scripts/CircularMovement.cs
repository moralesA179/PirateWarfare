using UnityEngine;

using UnityEngine.AI;

using static UnityEngine.GraphicsBuffer;



public class CircularMovement : MonoBehaviour

{

    public Transform player;

    public float orbitRadius = 10f;     // distance from player

    public float orbitSpeed = .5f;       // how fast it circles

    public float rotationSpeed = 360.0f;

    public float buffer = 2f;

    private bool isRevolving = false;

    private bool isBuffered = false;

    public bool canShoot = false;

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
            canShoot = false;
            agent.SetDestination(player.position);



            RotateTowards(player.position);

        }

        else if (playerDistance <= orbitRadius && !isBuffered)

        {

            isBuffered = true;

        }

        else if (isBuffered)

        {
            canShoot = true;
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

                // 1. Calculate the target direction and angle
                direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // 2. Create the target rotation
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

                // 3. Smoothly interpolate from current rotation to target rotation
                // Adjust rotationSpeed to control how fast it turns
                float rotationSpeed = 5f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            }



        }

    }

    void RotateTowards(Vector3 target)

    {

        Vector2 direction = target - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

}