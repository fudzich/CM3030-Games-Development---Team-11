using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShuriken : MonoBehaviour
{
    public GameObject rangedAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2")){
            Instantiate(rangedAttack, transform.position, transform.rotation);
        }
        
    }
}
