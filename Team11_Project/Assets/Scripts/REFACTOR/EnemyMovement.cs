using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float speed;

    private GameObject player;
    private GameObject spawner;
    private Camera mainCamera;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawner = GameObject.FindGameObjectWithTag("Spawner");
        if (spawner != null)
        {
            speed += spawner.GetComponent<ImproveEnemies>().getSpeed();
        }
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !player.GetComponent<Dodge>().IsInUse() && !GameController.Instance.IsGameOver())
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float moveMagnitude = direction.magnitude;

            // if (moveMagnitude > 0.1f)
            // {
            //     Quaternion targetRotation = Quaternion.LookRotation(direction);
            //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            // }

            // Move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            float horizontal = Vector3.Dot(direction, cameraRight);
            float vertical = Vector3.Dot(direction, cameraForward);

        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        float MGspeed = new Vector2(moveX, moveY).magnitude;

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetFloat("Speed", MGspeed);

    }
}



