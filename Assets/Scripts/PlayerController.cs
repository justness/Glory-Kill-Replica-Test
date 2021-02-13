using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    Scene scene;

    Rigidbody rb;
    float playerspeed = 130;
    int maxJump = 2;
    int jumpCounter;
    int jumpForce = 1800;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        transform.position = new Vector3(0, 0, 0); // Set spawn location.
        rb = GetComponent<Rigidbody>();
        jumpCounter = maxJump;

        scene = SceneManager.GetActiveScene();
    }

    void FixedUpdate()
    {
        //Player movement.
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(cam.transform.forward * playerspeed);
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-(cam.transform.right) * playerspeed);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-(cam.transform.forward) * playerspeed);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(cam.transform.right * playerspeed);
        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter > 0)
        {
            rb.AddForce(Vector3.up * jumpForce);
            jumpCounter--;
        }
        if (GetComponent<Rigidbody>().velocity.y == 0) jumpCounter = maxJump;

        if (rb.velocity.y < 0) rb.drag = 0;
        else rb.drag = 4;

        //Personal preference/QoL, player does not move if no input.
        if (!(Input.anyKey))
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        //Restart scene on loss or for debugging.
        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < -5)
        {
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
    }
}
