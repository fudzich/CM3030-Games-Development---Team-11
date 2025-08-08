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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        float speed = new Vector2(moveX, moveY).magnitude;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetFloat("Speed", speed);

        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }
}