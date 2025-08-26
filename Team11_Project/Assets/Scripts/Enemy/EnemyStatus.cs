using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{

    [SerializeField] private int exp = 1; // return exp to player when dies
    private GameObject player;

    [SerializeField]
    private float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Die()
    {
        AudioManager.Instance.Play(AudioManager.AudioType.Enemy_Death);
        player.GetComponent<PlayerExpGain>().AddEXP(exp);
        Destroy(gameObject);
    }
    public void receiveDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }

    }
    public float getDamage()
    {
        return damage;
    }
    public void setDamage(float damage)
    {
        this.damage = damage;
    }

}
