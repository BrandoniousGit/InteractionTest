using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    public GameObject gunHolder, gunHolderPos;
    public float moveSpeed, jumpForce, slowdownMulti;
    public bool grounded, crouching;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void UserInput()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(xMove, 0, zMove).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

            rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
        }

        else
        {
            rb.velocity = new Vector3(rb.velocity.x / slowdownMulti, rb.velocity.y, rb.velocity.z / slowdownMulti);
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 0.72f))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void Jumping()
    {

        if (!grounded)
        {
            slowdownMulti = 1.04f;
        }
        else
        {
            slowdownMulti = 1.18f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(0, jumpForce, 0);
        }
    }

    void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            crouching = true;
            moveSpeed /= 1.5f;
            gunHolderPos.transform.localPosition -= new Vector3(0, 0.05f, 0);
            transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
            transform.localPosition -= new Vector3(0, 0.2f, 0);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            crouching = false;
            moveSpeed *= 1.5f;
            gunHolderPos.transform.localPosition += new Vector3(0, 0.05f, 0);
            transform.localScale = new Vector3(transform.localScale.x, 1.2f, transform.localScale.z);
            transform.localPosition += new Vector3(0, 0.2f, 0);
        }
    }

    void GunHUDInfo()
    {
        gunHolder.transform.position = gunHolderPos.transform.position;
        gunHolder.transform.rotation = gunHolderPos.transform.rotation;
    }

    void Update()
    {
        Jumping();
        Crouching();
        CheckGrounded();
        GunHUDInfo();
    }

    void LateUpdate()
    {
        UserInput();
    }
}
