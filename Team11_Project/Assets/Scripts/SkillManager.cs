using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private int learnStealthLv = 1;
    [SerializeField]
    private int learnFireBallLv = 2;
    [SerializeField]
    private int learnTornadoLv = 3;
    private int skillPoints = 0;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void learnSkillbyLv(int level)
    {
        if (level == learnStealthLv)
        {
            // Code to learn Stealth skill
            Debug.Log("Learned Stealth skill!");
            return;
        }
        else if (level == learnFireBallLv)
        {
            // Code to learn Fire Ball skill
            Debug.Log("Learned Fire Ball skill!");
            return;
        }
        else if (level == learnTornadoLv)
        {
            // Code to learn Tornado skill
            Debug.Log("Learned Tornado skill!");
            return;
        }
        skillPoints++;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
