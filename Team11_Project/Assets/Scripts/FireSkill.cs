using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{
    public GameObject rangedAttack;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Quaternion spawnRotation = Quaternion.LookRotation(transform.forward);
            GameObject fireball = Instantiate(rangedAttack, transform.position, spawnRotation);
            FireBall fb = fireball.GetComponent<FireBall>();
            if (fb != null)
            {
                fb.SetDirection(transform.forward); 
            }
        }
    }
}