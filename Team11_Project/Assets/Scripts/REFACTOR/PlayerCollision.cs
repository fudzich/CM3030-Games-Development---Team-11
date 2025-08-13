using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    /* moved to PlayerStatus */
    [SerializeField]
    float maxHp;

    float hp;

    [SerializeField]
    public float recievedMeleeDamage;

    [SerializeField]
    private HealthBar healthBar;
    /* moved to PlayerStatus */

    private GameObject spawner;

    // Start is called before the first frame update

    // redo this logic
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

    // moved to playestatus, call only once to reduce memory usage
    void Update()
    {
        if (hp <= 0)
        {
            PlayerDied();
        }
    }


    // moved to PlayerDamageHandler
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
    // moved to playestatus

    private void PlayerDied()
    {
        GameController.Instance.GameOver();
        gameObject.SetActive(false);
    }

    // moved to playestatus
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
