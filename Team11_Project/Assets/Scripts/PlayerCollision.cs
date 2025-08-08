using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    float maxHp;

    float hp;

    [SerializeField]
    public float recievedMeleeDamage;

    [SerializeField]
    private HealthBar healthBar;

    private GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;

        spawner = GameObject.FindGameObjectWithTag("Spawner");
        if (spawner != null)
        {
            recievedMeleeDamage += spawner.GetComponent<ImproveEnemies>().getDamage();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            PlayerDied();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            //gameObject.GetComponent<PlayerExpGain>().getEXP(s);

            hp -= recievedMeleeDamage;
            // Debug.Log("I was hit! I have " + hp + " left.");

            if (healthBar != null)
            {
                healthBar.takeDamage(recievedMeleeDamage);
            }
            else
            {
                // Debug.LogError("HealthBar is null! Assign it in the Inspector.");
            }

            AudioManager.Instance.Play(AudioManager.AudioType.Player_Damage);
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

        if (healthBar != null)
        {
            healthBar.fullHealAndIncreaseOnLevelUp(maxHp);
        }
        else
        {
            // Debug.LogError("HealthBar is null! Assign it in the Inspector.");
        }
    }
}
