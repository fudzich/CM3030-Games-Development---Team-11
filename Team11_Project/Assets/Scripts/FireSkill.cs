using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour, ISkill
{
    private bool isEnabled = false;
    public GameObject rangedAttack;

    private bool isOnCooldown;
    private float currentCooldown;
    [SerializeField]
    private float maxCooldown;
    private float spreadAngle;
    private float fireBallAmount;

    void Start()
    {
        fireBallAmount = 3;
        currentCooldown = 0f;
        spreadAngle = 15f;
        isOnCooldown = false;
    }

    void Update()
    {
        if (!isEnabled) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isOnCooldown)
        {
            for (int i = 0; i < fireBallAmount; i++)
            {
                float angleOffset = (i - (fireBallAmount - 1f) / 2f) * spreadAngle;

                Quaternion offsetRotation = Quaternion.Euler(0f, angleOffset, 0f);
                Quaternion spawnRotation = transform.rotation * offsetRotation;

                GameObject fireball = Instantiate(rangedAttack, transform.position, spawnRotation);
                FireBall fb = fireball.GetComponent<FireBall>();
                if (fb != null)
                {
                    fb.SetDirection(spawnRotation * Vector3.forward);
                }
            }
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
    public void EnableSkill()
    {
        isEnabled = true;
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