using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlash : MonoBehaviour
{
    public GameObject meleeAttack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var newSlash = Instantiate(meleeAttack, transform.position, transform.rotation);
            //newSlash.transform.parent = gameObject.transform;
        }
    }
}
