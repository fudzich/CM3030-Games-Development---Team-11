using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField]
    float hp;

    [SerializeField]
    public float recievedMeleeDamage;

    [SerializeField]
    public float recievedRangedDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        if( hp <= 0){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "RangedAttack"){
            Debug.Log("Enemy was hit by shuriken!");
            hp -= recievedRangedDamage;
        }

        if (other.gameObject.tag == "MeleeAttack"){
            Debug.Log("Enemy was hit by slash!");
            hp -= recievedMeleeDamage;
        }
    }
}
