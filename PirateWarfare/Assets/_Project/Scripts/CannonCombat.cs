using Unity.VisualScripting;
using UnityEngine;

public class CannonCombat : MonoBehaviour
{

    Animator animator;
    public int side = 0; // 0 for left, 1 for right
    public Cannon cannon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cannon = GetComponent<Cannon>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(side) && !cannon.reloading) //left click
        {
            animator.SetTrigger("Shoot");
        }
    }

}
