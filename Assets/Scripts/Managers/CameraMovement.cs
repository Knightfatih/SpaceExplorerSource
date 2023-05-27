using UnityEngine;

namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public float movementSpeed = 10f;
        public float rotationSpeed = 100f;

        private float yaw = 0f;
        private float pitch = 0f;

        private void Update()
        {
            HandleInput();
            RotateCamera();
        }

        private void HandleInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float currentMovementSpeed = Input.GetKey(KeyCode.LeftShift) ? movementSpeed * 2f : movementSpeed;

            UpdateYaw(mouseX);
            UpdatePitch(mouseY);
            ClampPitch();

            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
            movement = transform.TransformDirection(movement);
            movement *= currentMovementSpeed * Time.deltaTime;
            transform.position += movement;
        }

        private void UpdateYaw(float mouseX)
        {
            yaw += mouseX * rotationSpeed * Time.deltaTime;
        }

        private void UpdatePitch(float mouseY)
        {
            pitch -= mouseY * rotationSpeed * Time.deltaTime;
        }

        private void ClampPitch()
        {
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }

        private void RotateCamera()
        {
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
