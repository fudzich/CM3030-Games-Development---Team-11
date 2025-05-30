using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    float hp;

    [SerializeField]
    public float recievedMeleeDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if( hp <= 0){
        //    Destroy(gameObject);
        //}
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Enemy"){
            hp -= recievedMeleeDamage;
            Debug.Log("I was hit! I have " + hp + " left.");
        }
    }
}
