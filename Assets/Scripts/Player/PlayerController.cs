using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    // Movement
    private float walkSpeed = 3.0f;
    private float gravity = -9.81f;
    private Vector3 movementVector = Vector3.zero;
    private Vector2 movementInput = Vector2.zero;

    // Rotation
    private float mouseSensitivityX = 2.0f;
    private float mouseSensitivityY = 1.5f;
    private float maxLookUp = -60.0f;
    private float maxLookDown = 80.0f;
    private float verticalRotation = 0.0f;

    private CharacterController playerController = null;
    private Transform playerHead = null;
    private Camera playerCamera = null;


    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerHead = transform.GetChild(0);
        playerCamera = playerHead.GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            ProcessMovement();
            ProcessRotation();

            MovePlayer();
        }
    }

    private void ProcessMovement()
    {
        movementInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float movementVectorY = movementVector.y;
        movementVector = (transform.TransformDirection(Vector3.forward) * movementInput.x) + (transform.TransformDirection(Vector3.right) * movementInput.y);
        movementVector.y = movementVectorY;
    }

    private void ProcessRotation()
    {
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalRotation = Mathf.Clamp(verticalRotation, maxLookUp, maxLookDown);
        playerHead.transform.localRotation = Quaternion.Euler(verticalRotation, 0.0f, 0.0f);

        transform.rotation *= Quaternion.Euler(0.0f, Input.GetAxis("Mouse X") * mouseSensitivityX, 0.0f);
    }

    private void MovePlayer()
    {
        if (!playerController.isGrounded)
        {
            movementVector.y += gravity * Time.deltaTime;
        }

        playerController.Move(movementVector * Time.deltaTime);
    }
}
