using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossSkills : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private GameObject sandParticle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(RollSkill());
        }
    }


    private void TriggerRollSkill()
    {
        StartCoroutine(RollSkill());
    }
    private IEnumerator RollSkill()
    {
        // start the states
        animator.SetBool("IsRolling", true);
        agent.isStopped = true;
        sandParticle.transform.position = transform.position;
        sandParticle.transform.rotation = transform.rotation * Quaternion.Euler(0f, -90f, 0f);
        sandParticle.SetActive(true);

        // reset the states
        yield return new WaitForSeconds(0.5f); // [TODO: hardcoding, fix later]
        animator.SetBool("IsRolling", false);
        agent.isStopped = false;
        yield return new WaitForSeconds(0.5f); // [TODO: hardcoding, fix later]
        sandParticle.SetActive(false);
    }

}