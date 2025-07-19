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

    // Start is called before the first frame update
    void Start()
    {
        hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        //if( hp <= 0){
        //    Destroy(gameObject);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            //gameObject.GetComponent<PlayerExpGain>().getEXP(s);

            hp -= recievedMeleeDamage;
            Debug.Log("I was hit! I have " + hp + " left.");

            if (healthBar != null)
            {
                healthBar.takeDamage(recievedMeleeDamage);
            }
            else
            {
                Debug.LogError("HealthBar is null! Assign it in the Inspector.");
            }
        }
    }

    public void increaseMaxHP(float maxHPInreaseValue)
    {
        maxHp += maxHPInreaseValue;
        hp = maxHp;

        if (healthBar != null)
        {
            healthBar.fullHealOnLevelUp(maxHp);
        }
        else
        {
            Debug.LogError("HealthBar is null! Assign it in the Inspector.");
        }
    }
}
