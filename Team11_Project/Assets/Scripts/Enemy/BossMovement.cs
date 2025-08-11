using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.speed = 5f;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null && agent != null && agent.isOnNavMesh && !agent.isStopped)
        {
            // Normal: chasing player
            // target player
            agent.SetDestination(player.position);

            // face player
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            // update running direction anmiator 
            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity.normalized);
            if (animator != null)
            {
                animator.SetFloat("MoveX", localVelocity.x);
                animator.SetFloat("MoveZ", localVelocity.z);
            }
        }
        else
        {
            // Special: Skill condition

        }
    }
}