using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    public GameObject knife;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (knife != null)
        {
            knife.SetActive(false);
        }
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
            if (knife != null && Input.GetButtonDown("Fire1"))
            {
                knife.SetActive(true);
                StartCoroutine(DisableKnifeAfterDelay(0.5f)); //  disable after 0.5 seconds
            }
        }


    }

    private IEnumerator DisableKnifeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (knife != null && knife.activeSelf)
        {
            knife.SetActive(false);
        }
    }
}