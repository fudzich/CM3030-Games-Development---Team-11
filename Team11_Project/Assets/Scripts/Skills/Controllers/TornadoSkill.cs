using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TornadoSkill : MonoBehaviour, ISkill
{

    private bool isEnabled = false;
    public GameObject tornadoPrefab;

    private bool isOnCooldown;
    private float currentCooldown;
    [SerializeField]
    private float maxCooldown = 5f;

    private bool isInUse;
    private float currentDuration;
    [SerializeField]
    private float maxDuration = 6f;

    private GameObject activeTornado;
    [SerializeField] private float spawnOffset = 10f;

    private PlayerMovement playerMovement;

    [SerializeField] private GameObject summoningRune;

    [SerializeField] private GameObject icon;
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite fireSprite;

    private Image iconImage;

    PlayerLook playerLook;

    void Start()
    {
        currentCooldown = 0f;
        currentDuration = 0f;
        isOnCooldown = false;
        isInUse = false;
        iconImage = icon.GetComponent<Image>();
    }

    void Update()
    {
        if (!isEnabled) return;

        if (Input.GetKeyDown(KeyCode.Alpha2) && !isOnCooldown && !isInUse)
        {
            ActivateTornado();
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

        // Handle duration and cancellation on movement
        if (isInUse)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0f)
            {
                CancelTornado();
            }

            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (vertical != 0 || horizontal != 0)
            {
                CancelTornado();
            }
        }
    }

    private void ActivateTornado()
    {
        GetComponent<PlayerLook>().OnFreeze();

        isInUse = true;
        currentDuration = maxDuration;
        isOnCooldown = true;
        currentCooldown = maxCooldown;

        summoningRune.SetActive(true);

        Vector3 currentSpawnPosition = transform.position + transform.forward * spawnOffset;
        activeTornado = Instantiate(tornadoPrefab, currentSpawnPosition, Quaternion.identity);

        var tornado = activeTornado.GetComponentsInChildren<TornadoDamage>(true);
        if (tornado == null || tornado.Length == 0)
        {
            Debug.LogError("No TornadoDamage found in tornadoPrefab hierarchy.");
            return;
        }

        foreach (var td in tornado)
            td.Init(this);
        AudioManager.Instance.Play(AudioManager.AudioType.Wind);

    }

    private void CancelTornado()
    {
        if (activeTornado != null)
        {
            Destroy(activeTornado);
            activeTornado = null;
        }

        summoningRune.SetActive(false);
        iconImage.sprite = baseSprite;
        isInUse = false;
        GetComponent<PlayerLook>().OnUnfreeze();

    }
    public void OnFire()
    {
        iconImage.sprite = fireSprite;
    }

    public void OnFireBallLevelChanged()
    {
        maxDuration += 2f;
        if (maxDuration > 20f)
            maxDuration = 20f;
        maxCooldown -= 0.5f;
        if (maxCooldown < 2f)
            maxCooldown = 2f;
    }



    public void EnableSkill()
    {
        isEnabled = true;
    }

    public bool IsInUse()
    {
        return isInUse;
    }

    public float GetCurrentDuration()
    {
        return currentDuration;
    }

    public float GetMaxDuration()
    {
        return maxDuration;
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