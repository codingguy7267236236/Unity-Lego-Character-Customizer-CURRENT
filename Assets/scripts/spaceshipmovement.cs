using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceshipmovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 3f;
    public float turnSpeed = 2f;
    public float verticalRotationSpeed = 2f;
    void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");

        Debug.Log("Vertical:" + verticalInput);

        // Move the spaceship forward and backward
        transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);

        // Rotate the spaceship left and right
        //transform.Rotate(Vector3.up * rotationSpeed * horizontalInput * Time.deltaTime);

        // Turn the spaceship along its forward axis
        float turnAmount = Mathf.Lerp(0, horizontalInput, Mathf.Abs(verticalInput));
        transform.Rotate(Vector3.forward * -turnAmount * turnSpeed);

        // Rotate the spaceship vertically based on mouse input
        transform.Rotate(Vector3.left * mouseYInput * verticalRotationSpeed);

        // Rotate the spaceship horizontally based on mouse input
        transform.Rotate(Vector3.up * mouseXInput * rotationSpeed);
    }
}
