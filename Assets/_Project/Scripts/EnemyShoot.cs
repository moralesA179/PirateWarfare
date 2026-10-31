using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Timing (Randomized)")]
    public float minFireRate = 0.8f;
    public float maxFireRate = 1.8f;
    private float nextFireTime = 0f;

    [Header("Behavior Settings")]
    public bool isBoss = false;

    private CircularMovement circularMovement;
    private Cannon[] cannons;

    void Start()
    {
        cannons = GetComponentsInChildren<Cannon>();
        circularMovement = GetComponent<CircularMovement>();
    }

    void Update()
    {
        // 1. Safety check
        if (circularMovement == null || cannons == null || cannons.Length < 2 || circularMovement.player == null)
            return;

        // 2. Timer check AND Reload check
        if (Time.time >= nextFireTime && circularMovement.canShoot)
        {
            if (!cannons[0].reloading && !cannons[1].reloading)
            {
                ExecuteShot();
                CalculateNextFireTime();
            }
        }
    }

    private void ExecuteShot()
    {
        Vector2 dirToPlayer = (circularMovement.player.position - transform.position).normalized;
        float cannon1Alignment = Vector2.Dot(cannons[0].transform.right, dirToPlayer);
        float cannon2Alignment = Vector2.Dot(cannons[1].transform.right, dirToPlayer);

        int bestCannonIndex = (cannon1Alignment > cannon2Alignment) ? 0 : 1;

        if (isBoss)
        {
            float aiRoll = Random.value;

            // BOSS LOGIC: 40% chance to fire BOTH sides.
            if (aiRoll <= 0.40f)
            {
                cannons[0].Shoot();
                cannons[1].Shoot();
            }
            else
            {
                cannons[bestCannonIndex].Shoot();
            }
        }
        else
        {
            // NORMAL ENEMY LOGIC: Always optimal single shot
            cannons[bestCannonIndex].Shoot();
        }
    }

    private void CalculateNextFireTime()
    {
        float randomDelay = Random.Range(minFireRate, maxFireRate);

        if (isBoss)
        {
            randomDelay *= 0.5f;
        }

        nextFireTime = Time.time + randomDelay;
    }
}