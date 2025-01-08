using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager = null;

    [Header("Options")]
    [SerializeField] private bool allowSprinting = true;
    [SerializeField] private bool allowJumping = true;

    [Header("Audio")]
    [SerializeField] private AudioClip collectItemSound = null;
    private AudioSource audioSource = null; 

    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => allowSprinting && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && playerController.isGrounded;

    // Movement
    private float walkSpeed = 2.0f;
    private float sprintSpeed = 4.5f;
    private float jumpForce = 4.5f;
    private float gravity = -9.81f;
    private Vector3 movementVector = Vector3.zero;
    private Vector2 movementInput = Vector2.zero;

    // Rotation
    private float mouseSensitivityX = 2.0f;
    private float mouseSensitivityY = 1.5f;
    private float maxLookUp = -60.0f;
    private float maxLookDown = 80.0f;
    private float verticalRotation = 0.0f;

    // Key Bindings
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode jumpKey = KeyCode.Space;

    private CharacterController playerController = null;
    private Transform playerHead = null;
    private Camera playerCamera = null;

    private IWeapon weapon = null;

    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerHead = transform.GetChild(0);
        playerCamera = playerHead.GetComponentInChildren<Camera>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (CanMove && gameManager.GetState() == GameState.RUNNING)
        {
            ProcessMovement();
            ProcessRotation();

            if (allowJumping)
            {
                ProcessJump();
            }

            ProcessShooting();

            MovePlayer();
        }
    }

    public void SetWeapon(IWeapon weapon)
    {
        audioSource.clip = collectItemSound;
        audioSource.Play();
        this.weapon = weapon;
    }

    private void ProcessMovement()
    {
        movementInput = new Vector2((IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

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

    private void ProcessJump()
    {
        if (ShouldJump)
        {
            movementVector.y = jumpForce;
        }
    }

    private void MovePlayer()
    {
        if (!playerController.isGrounded)
        {
            movementVector.y += gravity * Time.deltaTime;
        }

        playerController.Move(movementVector * Time.deltaTime);
    }

    private void ProcessShooting()
    {
        if (weapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (weapon is BalloonGun)
                {
                    weapon.Shoot();
                }
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (weapon is ShrinkRayGun)
                {
                    weapon.Shoot();
                }
            }
        }
    }
}
