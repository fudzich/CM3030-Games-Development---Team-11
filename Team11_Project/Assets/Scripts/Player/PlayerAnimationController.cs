using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    [Header("Weapon")]
    public GameObject knife;

    [Header("Skills")]
    [SerializeField] private TornadoSkill tornadoSkill;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (tornadoSkill == null)
            tornadoSkill = GetComponent<TornadoSkill>(); // fallback

        if (knife != null) knife.SetActive(false);
    }

    void Update()
    {
        // movement/attack 
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
                StartCoroutine(DisableKnifeAfterDelay(0.5f));
            }
        }

        // TornadoSkill summoning
        if (tornadoSkill != null)
        {
            bool inUse = tornadoSkill.IsInUse();
            if (inUse)
            {
                animator.SetTrigger("Summon");
            }
        }
    }

    public void DisableSummonAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Summon");
            animator.CrossFade("Idle", 0.1f, 0);

        }
    }


    private IEnumerator DisableKnifeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (knife != null && knife.activeSelf) knife.SetActive(false);
    }
}
