using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossSkills : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private GameObject sandParticle;
    public bool isUsingSkill { get; private set; }
    private float lastSkillTime;
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private float skillTriggerProbability = 0.5f;
    [SerializeField] private ParticleSystem fireParticle;

    private float defaultDamage;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        defaultDamage = GetComponent<EnemyStatus>().getDamage();
    }

    void Update()
    {
        if (!isUsingSkill)
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

    private void TriggerRoll()
    {
        StartCoroutine(RollRoutine());
        lastSkillTime = Time.time;
    }
    private IEnumerator RollRoutine()
    {
        // start the states
        isUsingSkill = true;
        animator.SetBool("IsRolling", true);
        agent.isStopped = true;
        sandParticle.transform.position = transform.position;
        sandParticle.transform.rotation = transform.rotation * Quaternion.Euler(0f, -90f, 0f);
        sandParticle.SetActive(true);
        GetComponent<EnemyStatus>().setDamage(defaultDamage * 2);

        // reset the states
        yield return new WaitForSeconds(0.5f); // [TODO: hardcoding, fix later]
        animator.SetBool("IsRolling", false);
        agent.isStopped = false;
        isUsingSkill = false;
        GetComponent<EnemyStatus>().setDamage(defaultDamage);

        yield return new WaitForSeconds(0.5f); // [TODO: hardcoding, fix later]
        sandParticle.SetActive(false);
    }
    private void TriggerSpin()
    {
        StartCoroutine(SpinRoutine());
        lastSkillTime = Time.time;
    }
    private IEnumerator SpinRoutine()
    {
        isUsingSkill = true;
        animator.SetBool("IsSpinning", true);
        agent.isStopped = true;
        fireParticle.Play();

        yield return new WaitForSeconds(1.5f);

        animator.SetBool("IsSpinning", false);
        agent.isStopped = false;
        fireParticle.Stop();
        isUsingSkill = false;

    }

}