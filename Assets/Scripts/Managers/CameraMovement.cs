using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float rotationSpeed = 100f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        float currentMovementSpeed = Input.GetKey(KeyCode.LeftShift) ? movementSpeed * 2f : movementSpeed;

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement = transform.TransformDirection(movement);
        movement *= currentMovementSpeed * Time.deltaTime;
        transform.position += movement;
    }
}
