using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField]
    private float hp;

    [SerializeField]
    public float recievedMeleeDamage;

    [SerializeField]
    public float recievedRangedDamage;

    [SerializeField]
    public float myEXPvalue;

    private GameObject player;
    private GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spawner = GameObject.FindGameObjectWithTag("Spawner");
        if (spawner != null)
        {
            hp += spawner.GetComponent<ImproveEnemies>().getHP();
            // Debug.Log(hp);
        }
    }

    void Update()
    {
        if (hp <= 0)
        {
            player.GetComponent<PlayerExpGain>().getEXP(myEXPvalue);
            Destroy(gameObject);
        }
    }
    public void receiveDamge(float damge)
    {
        hp -= damge;
    }
    public float getCurrentHP()
    {
        return hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RangedAttack")
        {
            // Debug.Log("Enemy was hit by shuriken!");
            hp -= recievedRangedDamage;
        }

        if (other.gameObject.tag == "MeleeAttack")
        {
            // Debug.Log("Enemy was hit by slash!");
            hp -= recievedMeleeDamage;
        }
    }
}
