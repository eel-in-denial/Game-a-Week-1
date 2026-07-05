using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float mouseSensitivity = 1f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Camera cam;
    private float verticalVelocity;
    private float cameraPitch = 0f;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse look
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime * 30f;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime * 30f;

        transform.Rotate(Vector3.up * mouseX);
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        cam.transform.localEulerAngles = Vector3.right * cameraPitch;

        // Movement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Gravity (no jump)
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}