using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpGain : MonoBehaviour
{
    private float exp;

    [SerializeField]
    float nextLevelRequirements;

    float currentLevel;

    [SerializeField]
    float speedInreaseValue;
    [SerializeField]
    float maxHPInreaseValue;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (exp >= nextLevelRequirements)
        {
            levelUp();
        }
    }


    public void getEXP(float value)
    {
        exp += value;
        Debug.Log("I earned " + value + " EXP!");
    }

    private void levelUp()
    {
        exp -= nextLevelRequirements;
        currentLevel++;
        Debug.Log("I levelled UP to" + currentLevel + " level!");

        //ststs increase:
        gameObject.GetComponent<PlayerMovement>().IncreaseSpeed(speedInreaseValue);
        gameObject.GetComponent<PlayerCollision>().increaseMaxHP(maxHPInreaseValue);
    }
}
