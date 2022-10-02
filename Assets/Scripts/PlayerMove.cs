using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    public float moveSpeed, maxSpeed;
    private float moveMulti;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void UserInput()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(xMove, 0, yMove).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveMulti = 1.5f;
        }
        else { moveMulti = 1.0f; }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        if (rb.velocity.magnitude >= maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x * maxSpeed, rb.velocity.y, rb.velocity.z * maxSpeed).normalized;
        }
    }

    void Update()
    {
        UserInput();
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            rb.velocity = new Vector3(rb.velocity.x / 1.2f, rb.velocity.y, rb.velocity.z / 1.2f);
        }
    }
}
