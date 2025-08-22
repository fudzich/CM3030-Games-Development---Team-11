using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Status target;
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth;
    public float health;
    private float lerpSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth = target.getCurrentHP();
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        easeHealthSlider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        health = target.getCurrentHP();

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void fullHealAndIncreaseOnLevelUp(float newMaxHpValue)
    {
        maxHealth = newMaxHpValue;
        health = maxHealth;
    }
}