using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    [SerializeField] private GameObject movementGuidePanel;
    [SerializeField] private GameObject rangedGuidePanel; 
    [SerializeField] private GameObject meleeGuidePanel;
    [SerializeField] private Vector3 guideOffset = new Vector3(0, 50, 0);  

    private Transform playerTransform; 
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerTransform = playerMovement.transform; 
        }
        else
        {
            Debug.LogError("PlayerMovement not found! Guide positioning and movement checks will fail.");
        }

        if (movementGuidePanel != null && playerTransform != null)
        {
            UpdateGuidePosition(movementGuidePanel);
            movementGuidePanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateGuidePosition(GameObject guidePanel)
    {
        if (playerTransform != null)
        {
            guidePanel.transform.position = playerTransform.position + guideOffset;
            //guidePanel.transform.rotation = Quaternion.LookRotation(Vector3.down);  
        }
    }
}
