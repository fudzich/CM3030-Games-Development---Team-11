using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private int learnStealthLv = 2;
    [SerializeField]
    private int learnFireBallLv = 3;
    [SerializeField]
    private int learnTornadoLv = 4;
    private int skillPoints = 0;

    [SerializeField]
    GameObject StealthUI;
    [SerializeField]
    GameObject FireBallUI;
    [SerializeField]
    GameObject TornadoUI;



    // Start is called before the first frame update
    void Start()
    {

    }
    public void learnSkillbyLv(int level)
    {
        if (level == learnStealthLv)
        {
            gameObject.GetComponent<Dodge>().EnableSkill();
            StealthUI.SetActive(true);
            return;
        }
        else if (level == learnFireBallLv)
        {
            gameObject.GetComponent<FireSkill>().EnableSkill();
            FireBallUI.SetActive(true);
            return;
        }
        else if (level == learnTornadoLv)
        {
            gameObject.GetComponent<TornadoSkill>().EnableSkill();
            TornadoUI.SetActive(true);
            return;
        }
        skillPoints++;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
