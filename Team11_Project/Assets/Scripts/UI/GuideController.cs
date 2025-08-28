using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    [SerializeField] private GameObject movementGuidePanel;
    [SerializeField] private GameObject rangedGuidePanel;
    [SerializeField] private GameObject meleeGuidePanel;
    [SerializeField] private GameObject stealthGuide;
    [SerializeField] private GameObject fireBallGuide;
    [SerializeField] private GameObject tornadoGuide;

    private bool hasMoved = false;
    private bool hasRightClicked = false;
    private bool hasLeftClicked = false;
    private bool stealthUsed = false;
    private bool fireBallUsed = false;
    private bool tornadoUsed = false;
    private Transform playerTransform;

    void Update()
    {
        if (!hasMoved)
        {
            // Debug.Log("checkingmovement");
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (vertical != 0 || horizontal != 0)
            {
                // Debug.Log("checked zero");
                hasMoved = true;
                movementGuidePanel.SetActive(false);
                rangedGuidePanel.SetActive(true);
            }
        }
        if (!hasRightClicked && hasMoved && Input.GetButtonDown("Fire2"))
        {
            hasRightClicked = true;
            rangedGuidePanel.SetActive(false);
            meleeGuidePanel.SetActive(true);
        }
        if (!hasLeftClicked && hasRightClicked && Input.GetButtonDown("Fire1"))
        {
            hasLeftClicked = true;
            meleeGuidePanel.SetActive(false);
        }
        if (!stealthUsed && Input.GetKeyDown(KeyCode.Space))
        {
            stealthUsed = true;
            stealthGuide.SetActive(false);
        }
        if (!fireBallUsed && Input.GetKeyDown(KeyCode.Alpha1))
        {
            fireBallUsed = true;
            fireBallGuide.SetActive(false);
        }
        if (!tornadoUsed && Input.GetKeyDown(KeyCode.Alpha2))
        {
            tornadoUsed = true;
            tornadoGuide.SetActive(false);
        }

    }

}
