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
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null && agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);

            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity.normalized);

            if (animator != null)
            {
                animator.SetFloat("MoveX", localVelocity.x);
                animator.SetFloat("MoveZ", localVelocity.z);
            }
        }
    }

    private System.Collections.IEnumerator CheckNavMeshPlacement()
    {
        yield return new WaitForEndOfFrame();
        if (agent != null && !agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                Debug.Log("Runtime warped to: " + hit.position);
            }
            else
            {
                Debug.LogError("Still no NavMesh position! Check bake.");
            }
        }
    }
}