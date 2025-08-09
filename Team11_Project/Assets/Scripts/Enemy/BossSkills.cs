using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossSkills : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private float skillTriggerProbability = 0.5f;
    private float lastSkillTime;
    private bool isUsingSkill;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found for skills!");
        }
    }

    void Update()
    {
        if (player != null && agent != null && !isUsingSkill)
        {
            CheckAndTriggerRandomSkill();
        }
    }

    private void CheckAndTriggerRandomSkill()
    {
        if (Time.time - lastSkillTime < skillCooldown) return;

        if (Random.value < skillTriggerProbability)
        {
            int randomSkill = Random.Range(0, 2);
            if (randomSkill == 0)
            {
                TriggerRoll();
            }
            else
            {
                TriggerSpin();
            }
        }
    }

    public void TriggerRoll()
    {
        if (isUsingSkill || Time.time - lastSkillTime < skillCooldown) return;

        StartCoroutine(RollCoroutine());
        lastSkillTime = Time.time;
    }

    private IEnumerator RollCoroutine()
    {
        isUsingSkill = true;
        if (animator != null)
        {
            animator.SetBool("IsRolling", true);
        }
        if (agent != null)
        {
            agent.speed = 10f;
        }

        yield return new WaitForSeconds(1f);

        if (animator != null)
        {
            animator.SetBool("IsRolling", false);
        }
        if (agent != null)
        {
            agent.speed = 5f;
        }
        isUsingSkill = false;
    }

    public void TriggerSpin()
    {
        if (isUsingSkill || Time.time - lastSkillTime < skillCooldown) return;

        StartCoroutine(SpinCoroutine());
        lastSkillTime = Time.time;
    }

    private IEnumerator SpinCoroutine()
    {
        isUsingSkill = true;
        if (animator != null)
        {
            animator.SetBool("IsSpinning", true);
        }
        if (agent != null)
        {
            agent.isStopped = true;
        }

        yield return new WaitForSeconds(1.5f);

        if (animator != null)
        {
            animator.SetBool("IsSpinning", false);
        }
        if (agent != null)
        {
            agent.isStopped = false;
        }
        isUsingSkill = false;
    }
}