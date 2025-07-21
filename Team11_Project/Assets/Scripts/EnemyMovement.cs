using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float speed;

    private GameObject player;
    private GameObject spawner;
    private Camera mainCamera; // For camera-relative direction
    private Animator animator; // For animation control

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
        if (animator == null)
        {
            Debug.LogWarning("Animator component is missing on Enemy! Add it in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !player.GetComponent<Dodge>().IsInUse() && !GameController.Instance.IsGameOver())
        {
            // Calculate direction to player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float moveMagnitude = direction.magnitude;

            // Rotate to face movement direction (smooth turn)
            if (moveMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust speed (5f) for faster/slower turn
            }

            // Move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            // Make direction relative to camera (for fixed camera angle animations)
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0f; // Flatten to XZ plane
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Project direction onto camera axes for "horizontal" and "vertical" relative to view
            float horizontal = Vector3.Dot(direction, cameraRight);
            float vertical = Vector3.Dot(direction, cameraForward);

            // Set Animator parameters safely (only if animator exists)
            if (animator != null)
            {
                animator.SetFloat("MoveX", horizontal);
                animator.SetFloat("MoveY", vertical);
                animator.SetFloat("Speed", moveMagnitude > 0.1f ? 1f : 0f); // 1 for moving, 0 for idle

                // Debug logs to check if parameters are setting (view in Console during Play)
                Debug.Log($"Enemy {name}: MoveX={horizontal}, MoveY={vertical}, Speed={animator.GetFloat("Speed")}");
            }
        }
    }
}