using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Camera camera;

    Rigidbody rb;

    [SerializeField]
    float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPoint = new Vector3(mousePos.x, mousePos.y, camera.transform.position.y - transform.position.y);
        Vector3 target = camera.ScreenToWorldPoint(worldPoint);
        transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        float vertical_movement = Input.GetAxisRaw("Vertical");
        float horizontal_movement = Input.GetAxisRaw("Horizontal");

        Vector3 newPos = new Vector3(horizontal_movement, 0, vertical_movement) * movementSpeed * Time.deltaTime;
        //transform.Translate(new Vector3(horizontal_movement, 0, vertical_movement) * movementSpeed * Time.deltaTime);

        rb.MovePosition(transform.position + newPos);

        if (vertical_movement == 0 && horizontal_movement == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

}
