using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : HealthBar
{
    private Transform cam;

    void Awake()
    {
        var main = Camera.main;
        if (main != null) cam = main.transform;

    }


    void LateUpdate()
    {
        // transform.LookAt(transform.position + cam.forward);
        transform.LookAt(transform.position + cam.forward, cam.up);
    }


}
