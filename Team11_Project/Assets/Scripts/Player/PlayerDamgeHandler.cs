using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    private PlayerStatus playerStatus;
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }


    // putting collision detection here but not enemy to reduce memory usage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            float dmg = collision.gameObject.GetComponent<EnemyStatus>().getDamage();
            playerStatus.receiveDamage(dmg);
        }
    }

}
