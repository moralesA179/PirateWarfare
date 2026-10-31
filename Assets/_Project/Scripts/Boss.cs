using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = 1.5f; // Time in seconds between shots
    public Transform t;

    private CircularMovement circularMovement; // Note: Change to PirateShipMovement if you updated the script name
    private Cannon[] cannons;
    private bool isShootingRoutineRunning = false;

    void Start()
    {
        cannons = GetComponentsInChildren<Cannon>();
        circularMovement = GetComponent<CircularMovement>();
    }

    void Update()
    {
        // If the enemy is allowed to shoot, and the routine isn't already running, start it
        if (circularMovement != null && circularMovement.canShoot && !isShootingRoutineRunning)
        {
            StartCoroutine(PeriodicFireRoutine());
        }
    }

    private IEnumerator PeriodicFireRoutine()
    {
        isShootingRoutineRunning = true;

        // Keep looping and shooting as long as the ship is allowed to shoot
        while (circularMovement.canShoot)
        {
            // Check direction to fire the correct cannon
            if (t.right.x > 0)
            {
                cannons[0].Shoot();
            }
            else if (t.right.x < 0)
            {
                cannons[1].Shoot();
            }

            // Pause this specific function for the fireRate duration before looping again
            yield return new WaitForSeconds(fireRate);
        }

        // If canShoot becomes false, the loop naturally breaks, and we reset the flag
        isShootingRoutineRunning = false;
    }
}