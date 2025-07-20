using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{

    [SerializeField]
    private float maxDodgeDuration;

    [SerializeField]
    private float maxDodgeCooldown;

    float currentDodgeDuration;

    float currentDodgeCooldown;

    bool isinDodge;
    bool isOnCooldown;

    [SerializeField]
    GameObject playersModel;

    [SerializeField]
    GameObject shadowForm;


    // Start is called before the first frame update
    void Start()
    {
        currentDodgeDuration = maxDodgeDuration;
        currentDodgeCooldown = maxDodgeCooldown;

        isinDodge = false;
    }

    // Update is called once per frame
    void Update()
    {

        CheckCooldown();

        if (Input.GetKeyDown(KeyCode.Space) && !isinDodge && !isOnCooldown)
        {
            DoADodge();
        }
    }

    private void FixedUpdate()
    {
        if (currentDodgeDuration <= 0)
        {
            isinDodge = false;
            currentDodgeDuration = maxDodgeDuration;
            ToggleModel();
            Debug.Log("My dodge has ended!");

            isOnCooldown = true;
        }
        else if (isinDodge)
        {
            currentDodgeDuration--;
            Debug.Log("I am dodging and I have " + currentDodgeDuration + " frames left!");
        }
    }

    private void DoADodge()
    {
        isinDodge = true;
        ToggleModel();
    }

    private void ToggleModel()
    {
        playersModel.SetActive(!playersModel.activeSelf);
        shadowForm.SetActive(!shadowForm.activeSelf);
    }

    private void CheckCooldown()
    {
        if (isOnCooldown)
        {
            currentDodgeCooldown--;
            if (currentDodgeCooldown <= 0)
            {
                currentDodgeCooldown = maxDodgeCooldown;
                isOnCooldown = false;
            }
        }
    }

    public bool IsinDodge()
    {
        return isinDodge;
    }
}
