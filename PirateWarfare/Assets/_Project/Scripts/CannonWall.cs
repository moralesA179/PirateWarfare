using UnityEngine;

public class CannonWall : MonoBehaviour
{
    Cannon cannon;
    Animator animator;

    void Start()
    {
        cannon = GetComponentInChildren<Cannon>();
        animator = GetComponentInChildren<Animator>();
        InvokeRepeating("ShootCannon", 2f, 3f);
    }

    void ShootCannon()
    {
        cannon.Shoot();
        animator.SetTrigger("Shoot");
    }
}
