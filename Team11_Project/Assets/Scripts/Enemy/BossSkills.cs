using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossSkills : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private GameObject sandParticlePrefab;

    private GameObject sandFX;
    private ParticleSystem sandPS;
    public bool isUsingSkill { get; private set; }
    private float lastSkillTime;
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private float skillTriggerProbability = 0.5f;
    [SerializeField] private ParticleSystem fireParticle;

    private CapsuleCollider capsule;
    [SerializeField] private float normalRadius = 0.5f;
    [SerializeField] private float skillRadius = 1.2f;

    private float defaultDamage;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        defaultDamage = GetComponent<EnemyStatus>().getDamage();
        capsule = GetComponent<CapsuleCollider>();

        if (sandParticlePrefab != null)
        {
            sandFX = Instantiate(sandParticlePrefab, transform);
            sandFX.transform.localPosition = Vector3.zero;
            sandFX.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
            sandPS = sandFX.GetComponent<ParticleSystem>();
            sandFX.SetActive(false);
        }

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
        // show sand
        if (sandFX)
        {
            sandFX.SetActive(true);
            if (sandPS) { sandPS.Clear(true); sandPS.Play(true); }
        }
        GetComponent<EnemyStatus>().setDamage(defaultDamage * 2);

        // reset the states
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsRolling", false);
        agent.isStopped = false;
        isUsingSkill = false;
        GetComponent<EnemyStatus>().setDamage(defaultDamage);

        yield return new WaitForSeconds(0.5f);
        if (sandPS) sandPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (sandFX) sandFX.SetActive(false);
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
        fireParticle.Clear(true);
        fireParticle.Play(true);
        SetCapsuleCollider(skillRadius);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsSpinning", false);
        agent.isStopped = false;
        isUsingSkill = false;

        yield return new WaitForSeconds(4f);
        // fireParticle.Stop();
        fireParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        SetCapsuleCollider(normalRadius);

    }

    private void SetCapsuleCollider(float radius)
    {
        capsule.radius = radius;
    }


}