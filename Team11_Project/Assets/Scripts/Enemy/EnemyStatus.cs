using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]
    private float hp;

    [SerializeField]
    public float exp { get; private set; }
    private GameObject player;
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.Play(AudioManager.AudioType.Enemy_Death);
        player.GetComponent<PlayerExpGain>().getEXP(exp); //[TODO, refactor to use a more generic method]
        Destroy(gameObject);
    }
    public void receiveDamage(float damage)
    {
        hp -= damage;
    }
    public float getCurrentHP()
    {
        return hp;
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
