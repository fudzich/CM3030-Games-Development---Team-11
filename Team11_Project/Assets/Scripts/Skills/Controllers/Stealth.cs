using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : MonoBehaviour, ISkill
{
    [Header("Settings (ms)")]
    [SerializeField] private float maxStealthDuration =300;
    [SerializeField] private float maxStealthCooldown = 800;

    [Header("Visuals")]
    [SerializeField] GameObject playersModel;
    [SerializeField] GameObject shadowForm;

    private bool isEnabled = false;
    bool isInStealth;
    bool isOnCooldown;
    float currentStealthDuration;
    float currentStealthCooldown;

    // Start is called before the first frame update
    void Start()
    {
        currentStealthDuration = maxStealthDuration;
        currentStealthCooldown = maxStealthCooldown;
        isInStealth = false;
        isOnCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;
        CheckCooldown();

        if (Input.GetKeyDown(KeyCode.Space) && !isInStealth && !isOnCooldown)
        {
            DoAStealth();
        }
    }

    private void FixedUpdate()
    {
        if (currentStealthDuration <= 0)
        {
            isInStealth = false;
            currentStealthDuration = maxStealthDuration;
            ToggleModel();
            // Debug.Log("My stealth has ended!");

            isOnCooldown = true;
        }
        else if (isInStealth)
        {
            currentStealthDuration--;
            // Debug.Log("I am dodging and I have " + currentStealthDuration + " frames left!");
        }
    }

    private void DoAStealth()
    {
        isInStealth = true;
        ToggleModel();
        AudioManager.Instance.Play(AudioManager.AudioType.Invisibility);

    }

    private void ToggleModel()
    {
        playersModel.SetActive(!playersModel.activeSelf);
        shadowForm.SetActive(!shadowForm.activeSelf);
    }

    private void CheckCooldown()
    {
        if (isOnCooldown)
        {
            currentStealthCooldown--;
            if (currentStealthCooldown <= 0)
            {
                currentStealthCooldown = maxStealthCooldown;
                isOnCooldown = false;
            }
        }
    }

    public void OnStealthLevelChanged()
    {
        currentStealthDuration += 100;
        currentStealthCooldown -= 100;
    }

    public void EnableSkill()
    {
        isEnabled = true;
    }

    public bool IsInUse()
    {
        return isInStealth;
    }

    public float GetCurrentDuration()
    {
        return currentStealthDuration;
    }

    public float GetMaxDuration()
    {
        return maxStealthDuration;
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCurrentCooldown()
    {
        return currentStealthCooldown;
    }

    public float GetMaxCooldown()
    {
        return maxStealthCooldown;
    }
}