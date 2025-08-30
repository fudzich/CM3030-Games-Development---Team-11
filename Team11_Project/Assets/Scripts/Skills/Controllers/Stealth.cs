using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : MonoBehaviour, ISkill
{
    [Header("Settings (seconds)")]
    [SerializeField] private float maxDodgeDuration = 0.3f;
    [SerializeField] private float maxDodgeCooldown = 0.8f;
    [SerializeField] private KeyCode dodgeKey = KeyCode.Space;

    [Header("Visuals")]
    [SerializeField] private GameObject playersModel;
    [SerializeField] private GameObject shadowForm;

    private bool isEnabled = false;
    private bool isDodging = false;
    private bool isOnCooldown = false;
    private float durTimer = 0f;
    private float cdTimer = 0f;

    void Start()
    {
        // Ensure known starting state
        SetVisible(true);      // player visible, shadow hidden
        isDodging = false;
        isOnCooldown = false;
        durTimer = 0f;
        cdTimer = 0f;
    }

    void Update()
    {
        if (!isEnabled) return;

        // Start dodge
        if (Input.GetKeyDown(dodgeKey) && !isDodging && !isOnCooldown)
        {
            isDodging = true;
            durTimer = maxDodgeDuration; // seconds
            SetVisible(false);           // hide player, show shadow
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play(AudioManager.AudioType.Invisibility);
        }

        // Cooldown tick (seconds)
        if (isOnCooldown)
        {
            cdTimer -= Time.deltaTime;
            if (cdTimer <= 0f)
            {
                cdTimer = 0f;
                isOnCooldown = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDodging) return;

        // Dodge duration tick (seconds)
        durTimer -= Time.fixedDeltaTime;
        if (durTimer <= 0f)
        {
            isDodging = false;
            SetVisible(true);          // back to normal
            isOnCooldown = true;
            cdTimer = maxDodgeCooldown;
        }
    }

    private void SetVisible(bool visible)
    {
        if (playersModel) playersModel.SetActive(visible);
        if (shadowForm) shadowForm.SetActive(!visible);
    }

    public void OnStealthLevelChanged()
    {
        maxDodgeDuration += 0.1f;
        if (maxDodgeDuration > 1f)
            maxDodgeDuration = 1f;
        maxDodgeCooldown -= 0.1f;
        if (maxDodgeCooldown < 0.4f)
            maxDodgeCooldown = 0.4f;
    }

    // ISkill
    public void EnableSkill() => isEnabled = true;
    public bool IsInUse() => isDodging;
    public float GetCurrentDuration() => Mathf.Max(durTimer, 0f);
    public float GetMaxDuration() => maxDodgeDuration;
    public bool IsOnCooldown() => isOnCooldown;
    public float GetCurrentCooldown() => isOnCooldown ? Mathf.Max(cdTimer, 0f) : 0f;
    public float GetMaxCooldown() => maxDodgeCooldown;
}
