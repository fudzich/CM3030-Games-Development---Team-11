using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage = 15f;  // Damage dealt to enemies

    private Vector3 moveDirection;  // Direction set at spawn

    // Called by the spawning script to set direction
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        // Rotate to face the direction
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("enemy colli with fireball");

        if (other.CompareTag("Enemy"))
        {
            // Debug.Log("enemy colli with fireball enemy");
            // Apply damage to the enemy
            EnemyCollision enemy = other.GetComponent<EnemyCollision>();
            if (enemy != null)
            {
                enemy.receiveDamge(damage);
                Debug.Log($"Fireball hit {other.name}! Enemy HP: {enemy.getCurrentHP()}");
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);  // Move along local forward
    }
}