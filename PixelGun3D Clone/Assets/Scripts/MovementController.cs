using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float lookSensitivity = 3f;
    [SerializeField] GameObject fpsCamera;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraUpAndDownRotation = 0f;
    private float currentCameraUpAndDownRotation = 0f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float _xMovement = Input.GetAxis("Horizontal");
        float _zMovement = Input.GetAxis("Vertical");

        Vector3 _movementHorizontal = transform.right * _xMovement;
        Vector3 _movementVertical = transform.forward * _zMovement;

        // Final movement Velocity
        Vector3 _movementVelocity = (_movementHorizontal + _movementVertical).normalized * speed;
        SetMovementVelocity(_movementVelocity);

        // Calculate Rotation
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotationVector = new Vector3(0, _yRotation, 0) * lookSensitivity;
        // Apply Rotation
        setRotation(_rotationVector);
        // Calculate Up/Down Camera rotation
        float _cameraUpDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;
        setCameraRotation(_cameraUpDownRotation);

    }

    private void FixedUpdate()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }    
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if(fpsCamera != null)
        {
            currentCameraUpAndDownRotation -= cameraUpAndDownRotation;
            currentCameraUpAndDownRotation = Mathf.Clamp(currentCameraUpAndDownRotation, -85f, 85f);
            fpsCamera.transform.localEulerAngles = new Vector3(currentCameraUpAndDownRotation, 0, 0);
        }
    }

    private void SetMovementVelocity(Vector3 movementVelocity)
    {
        velocity = movementVelocity;
    }

    private void setRotation(Vector3 rotationVector)
    {
        rotation = rotationVector;
    }

    private void setCameraRotation(float cameraUpAndDownRotation)
    {
        this.cameraUpAndDownRotation = cameraUpAndDownRotation;
    }
}
