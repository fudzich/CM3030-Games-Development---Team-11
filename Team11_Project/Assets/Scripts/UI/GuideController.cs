using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    [SerializeField] private GameObject movementGuidePanel;
    [SerializeField] private GameObject rangedGuidePanel;
    [SerializeField] private GameObject meleeGuidePanel;

    private bool hasMoved = false;
    private bool hasRightClicked = false;
    private bool hasLeftClicked = false;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
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



    }

}
