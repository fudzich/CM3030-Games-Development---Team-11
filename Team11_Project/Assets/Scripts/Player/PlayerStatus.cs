using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    float maxHp;

    float hp;
    [SerializeField]
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void receiveDamage(float damage)
    {
        hp -= damage;
        healthBar.takeDamage(damage);
        AudioManager.Instance.Play(AudioManager.AudioType.Player_Damage);

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

}
