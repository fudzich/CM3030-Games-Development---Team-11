using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1"))

        {
            animator.SetTrigger("Attack");
        }


    }
}