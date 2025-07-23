using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour, ISkill
{
    public GameObject rangedAttack;

    private bool isOnCooldown;
    private float currentCooldown;
    [SerializeField]
    private float maxCooldown;  

    void Start()
    {
        currentCooldown = 0f;  
        isOnCooldown = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isOnCooldown)
        {
            Quaternion spawnRotation = Quaternion.LookRotation(transform.forward);
            GameObject fireball = Instantiate(rangedAttack, transform.position, spawnRotation);
            FireBall fb = fireball.GetComponent<FireBall>();
            if (fb != null)
            {
                fb.SetDirection(transform.forward);
            }

            // Start cooldown
            currentCooldown = maxCooldown;
            isOnCooldown = true;
        }

        // Handle cooldown timer
        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                currentCooldown = 0f;
                isOnCooldown = false;
            }
        }
    }

    public bool IsInUse()
    {
        return false; 
    }

    public float GetCurrentDuration()
    {
        return 0f;
    }

    public float GetMaxDuration()
    {
        return 0f;
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCurrentCooldown()
    {
        return currentCooldown;
    }

    public float GetMaxCooldown()
    {
        return maxCooldown;
    }
}