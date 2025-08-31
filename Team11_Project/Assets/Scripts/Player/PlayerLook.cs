using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    bool isFreeze = false;

    [SerializeField] float rotateSpeed = 999f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Update()
    {
        if (isFreeze) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // Horizontal plane at the player's current Y
        Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

        if (aimPlane.Raycast(ray, out float dist))
        {
            Vector3 world = ray.GetPoint(dist);
            Vector3 lookDir = new Vector3(world.x, transform.position.y, world.z) - transform.position;

            if (lookDir.sqrMagnitude > 0.0001f)
            {
                Quaternion target = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotateSpeed);
            }
        }
    }

    public void OnFreeze() { isFreeze = true; rb.velocity = Vector3.zero; }
    public void OnUnfreeze() { isFreeze = false; }
}
