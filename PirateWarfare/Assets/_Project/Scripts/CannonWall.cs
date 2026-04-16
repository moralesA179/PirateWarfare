using UnityEngine;
using System.Collections;

public class CannonWall : MonoBehaviour
{
    Cannon cannon;
    Animator animator;

    void Start()
    {
        cannon = GetComponentInChildren<Cannon>();
        cannon.enemy = true;
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(RandomShootRoutine());
    }

    IEnumerator RandomShootRoutine()
    {
        // Initial delay before the first shot
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        while (true)
        {
            ShootCannon();

            // Pick a new random time to wait before the next shot
            float nextWaitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(nextWaitTime);
        }
    }

    void ShootCannon()
    {
        cannon.Shoot();
        animator.SetTrigger("Shoot");
    }
}
