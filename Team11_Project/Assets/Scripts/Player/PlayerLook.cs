using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    Rigidbody rb;

    Camera cam;

    Ray mouseRay;

    Vector3 hitPoint;

    bool isFreeze = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreeze)
        {
            // Debug.Log("Frozen");
            return;
        }
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(mouseRay, out hitInfo, 100f))
        {
            hitPoint = hitInfo.point;

        }

        Vector3 lookTarget = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);

        transform.LookAt(lookTarget);
    }

    public void OnFreeze()
    {
        isFreeze = true;
        rb.velocity = Vector3.zero;
    }

    public void OnUnfreeze()
    {
        isFreeze = false;
    }
}


