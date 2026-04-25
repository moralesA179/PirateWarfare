using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float fireRate = 1.5f; // Time in seconds between shots
    public Transform t;

    private CircularMovement circularMovement;
    private Cannon[] cannons;
    private bool isShootingRoutineRunning = false;

    void Start()
    {
        cannons = GetComponentsInChildren<Cannon>();
        circularMovement = GetComponent<CircularMovement>();
    }

    void Update()
    {
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

            yield return new WaitForSeconds(fireRate);
        }

        isShootingRoutineRunning = false;
    }
}