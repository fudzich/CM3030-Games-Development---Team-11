using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, ISkill
{
    private bool isEnabled = false;
    private bool isOnCooldown;
    private float currentCooldown;
    [SerializeField] private float maxCooldown = 5f;

    [SerializeField] GameObject healAnimation;

    [SerializeField] float healthAomunt = 50f;

    void Update()
    {
        if (!isEnabled) return;

        if (Input.GetKeyDown(KeyCode.Alpha3) && !isOnCooldown)
        {
            DoHeal();
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

    private void DoHeal()
    {
        PlayerStatus playerStatus = GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.heal(healthAomunt);
            isOnCooldown = true;
            currentCooldown = maxCooldown;
            PlayHeal();
            AudioManager.Instance.Play(AudioManager.AudioType.Heal);

        }
    }

    public void PlayHeal()
    {
        StartCoroutine(ShowTemporarily(healAnimation, 1f));
    }
    IEnumerator ShowTemporarily(GameObject go, float seconds)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(seconds);
        go.SetActive(false);
    }

    public void OnHealLevelChanged()
    {
        maxCooldown -= 0.5f;
        if (maxCooldown < 2f)
            maxCooldown = 2f;
        healthAomunt += 10f;
        if (healthAomunt > 100f)
            healthAomunt = 100f;
    }


    public bool IsInUse() => false;
    public void EnableSkill() => isEnabled = true;
    public float GetCurrentDuration() => 0f;
    public float GetMaxDuration() => 0f;
    public bool IsOnCooldown() => isOnCooldown;
    public float GetCurrentCooldown() => currentCooldown;
    public float GetMaxCooldown() => maxCooldown;

}
