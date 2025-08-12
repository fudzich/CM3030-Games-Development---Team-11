using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImproveEnemies : MonoBehaviour
{
    [SerializeField]
    float hpImprovementPerFrame;

    [SerializeField]
    float damageImprovementPerFrame;

    [SerializeField]
    float speedImprovementPerFrame;

    private float currentIncreaseInHP;
    private float currentIncreaseInDamage;
    private float currentIncreaseInSpeed;


    // Start is called before the first frame update
    void Start()
    {
        currentIncreaseInHP = 0;
        currentIncreaseInDamage = 0;
        currentIncreaseInSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentIncreaseInHP += hpImprovementPerFrame;
        currentIncreaseInDamage += damageImprovementPerFrame;
        currentIncreaseInSpeed += speedImprovementPerFrame;
    }

    public float getHP()
    {
        return currentIncreaseInHP;
    }

    public float getDamage()
    {
        return currentIncreaseInDamage;
    }

    public float getSpeed()
    {
        return currentIncreaseInSpeed;
    }
}
