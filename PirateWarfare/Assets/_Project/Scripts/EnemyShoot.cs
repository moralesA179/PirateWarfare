using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Timing (Randomized)")]
    public float minFireRate = 0.8f;
    public float maxFireRate = 1.8f;
    private float nextFireTime = 0f;

    [Header("Behavior Settings")]
    public bool isBoss = false;

    [Tooltip("Chance (0.0 to 1.0) a normal ship fires both sides at once")]
    public float doubleFireChance = 0.2f; // 20% chance

    [Tooltip("Chance (0.0 to 1.0) a normal ship panics and shoots the wrong way")]
    public float mistakeChance = 0.1f; // 10% chance

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
        // We now force the AI to wait until BOTH cannons are done reloading from their own script
        if (Time.time >= nextFireTime && circularMovement.canShoot)
        {
            if (!cannons[0].reloading && !cannons[1].reloading)
            {
                ExecuteUnpredictableShot();
                CalculateNextFireTime();
            }
        }
    }

    private void ExecuteUnpredictableShot()
    {
        Vector2 dirToPlayer = (circularMovement.player.position - transform.position).normalized;
        float cannon1Alignment = Vector2.Dot(cannons[0].transform.right, dirToPlayer);
        float cannon2Alignment = Vector2.Dot(cannons[1].transform.right, dirToPlayer);

        int bestCannonIndex = (cannon1Alignment > cannon2Alignment) ? 0 : 1;
        int wrongCannonIndex = (bestCannonIndex == 0) ? 1 : 0;

        float aiRoll = Random.value;

        if (isBoss)
        {
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
            // NORMAL ENEMY LOGIC
            if (aiRoll <= doubleFireChance)
            {
                // Action: All out attack! Fire both!
                cannons[0].Shoot();
                cannons[1].Shoot();
            }
            else if (aiRoll <= doubleFireChance + mistakeChance)
            {
                // Action: Panic! Fire ONLY the wrong side!
                cannons[wrongCannonIndex].Shoot();
            }
            else
            {
                // Action: Standard optimal single shot
                cannons[bestCannonIndex].Shoot();
            }
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