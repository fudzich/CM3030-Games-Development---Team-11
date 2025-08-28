using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField]
    float maxHp;

    // float hp;
    [SerializeField]
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public void receiveDamage(float damage)
    {
        hp = Mathf.Clamp(hp - damage, 0f, maxHp);
        // healthBar.takeDamage(damage);
        AudioManager.Instance.Play(AudioManager.AudioType.Player_Damage);
        // call checker on action instead of every frame to reduce memory usage
        if (hp <= 0)
        {
            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        GameController.Instance.GameOver();
        gameObject.SetActive(false);
    }

    public void increaseMaxHP(float maxHPInreaseValue)
    {
        maxHp += maxHPInreaseValue;
        hp = maxHp;
        healthBar.fullHealAndIncreaseOnLevelUp(maxHp);
    }

    public void heal(float healValue)
    {
        hp = Mathf.Clamp(hp + healValue, 0f, maxHp);
        healthBar.heal(healValue);
    }



}
