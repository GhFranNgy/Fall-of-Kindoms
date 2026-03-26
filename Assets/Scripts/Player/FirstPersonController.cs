using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    // ================= REFERENCES =================
    [Header("References")]
    public UserSettings userSettings;
    public Transform playerCamera;
    public Transform groundCheck;

    [Header("Mobile Joysticks (optional)")]
    public FloatingJoystick leftJoystick; 

    [Header("Mobile Look Panel (optional)")]
    public RectTransform rightLookPanel;
    public float touchLookSensitivity = 0.1f;

    public bool isAndroid;

    // ================= LOOK =================
    [Header("Mouse Look")]
    public float smoothSpeed = 15f;

    // ================= MOVEMENT =================
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 3f;
    public float gravity = 9.81f;

    // ================= CROUCH =================
    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float crouchCameraOffset = -0.5f;
    public float crouchSmoothSpeed = 10f;

    // ================= HEAD BOB =================
    [Header("Head Bob")]
    public bool enableHeadBob = true;
    public float bobSpeed = 10f;
    public float bobAmount = 0.05f;

    // ================= GROUND =================
    [Header("Ground Check")]
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    // ================= INTERNAL =================
    private CharacterController controller;
    private Vector3 velocity;

    private float yaw;
    private float pitch;
    private float currentYaw;
    private float currentPitch;

    private float originalHeight;
    private float originalCameraY;
    private float targetCameraY;

    private bool isGrounded;
    private bool isCrouching;

    private float bobTimer;

    // ================= UNITY =================
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;

        // Initialize camera rotation
        yaw = transform.eulerAngles.y;
        pitch = 0f;
        currentYaw = yaw;
        currentPitch = pitch;

        playerCamera.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);

        originalCameraY = playerCamera.localPosition.y;
        targetCameraY = originalCameraY;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    // ================= LOOK =================
    private void HandleLook()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        // Desktop mouse input
        if (!isAndroid)
        {
            mouseX = Input.GetAxis(userSettings.mouseXAxis) * userSettings.mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis(userSettings.mouseYAxis) * userSettings.mouseSensitivity * Time.deltaTime;
            if (userSettings.invertMouseY) mouseY *= -1f;
        }

        // Mobile right-panel swipe
        if (isAndroid && rightLookPanel != null && Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (RectTransformUtility.RectangleContainsScreenPoint(rightLookPanel, touch.position))
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 delta = touch.deltaPosition;
                        mouseX = delta.x * touchLookSensitivity;
                        mouseY = delta.y * touchLookSensitivity;
                        if (userSettings.invertMouseY) mouseY *= -1f;
                    }
                }
            }
        }

        // Apply rotation
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        currentYaw = Mathf.Lerp(currentYaw, yaw, smoothSpeed * Time.deltaTime);
        currentPitch = Mathf.Lerp(currentPitch, pitch, smoothSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        playerCamera.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
    }

    // ================= MOVEMENT =================
    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector2 moveInput;

        if (isAndroid)
        {
            // Mobile joystick movement
            moveInput = new Vector2(leftJoystick.Horizontal(), leftJoystick.Vertical());
        }
        else
        {
            // Keyboard movement
            moveInput = Vector2.zero;
            if (Input.GetKey(userSettings.move_Forward)) moveInput.y += 1f;
            if (Input.GetKey(userSettings.move_Backward)) moveInput.y -= 1f;
            if (Input.GetKey(userSettings.move_Right)) moveInput.x += 1f;
            if (Input.GetKey(userSettings.move_Left)) moveInput.x -= 1f;
        }

        isCrouching = Input.GetKey(userSettings.move_Crouch) || (leftJoystick != null && moveInput.magnitude > 0 && isCrouching);

        float speed = isCrouching ? crouchSpeed : walkSpeed;

        Vector3 move = transform.TransformDirection(new Vector3(moveInput.x, 0f, moveInput.y));
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(userSettings.move_Jump) && isGrounded)
            velocity.y = jumpForce;

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        HandleCrouch();
        HandleHeadBob(moveInput);
    }

    // ================= CROUCH =================
    private void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);
        controller.center = new Vector3(0f, controller.height / 2f, 0f);

        targetCameraY = isCrouching ? originalCameraY + crouchCameraOffset : originalCameraY;
        float newY = Mathf.Lerp(playerCamera.localPosition.y, targetCameraY, Time.deltaTime * crouchSmoothSpeed);
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, newY, playerCamera.localPosition.z);
    }

    // ================= HEAD BOB =================
    private void HandleHeadBob(Vector2 input)
    {
        if (!enableHeadBob) return;

        if (!isGrounded || isCrouching || input.magnitude < 0.1f)
        {
            bobTimer = 0f;
            float y = Mathf.Lerp(playerCamera.localPosition.y, targetCameraY, Time.deltaTime * 10f);
            playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, y, playerCamera.localPosition.z);
            return;
        }

        bobTimer += Time.deltaTime * bobSpeed;
        float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, targetCameraY + bobOffset, playerCamera.localPosition.z);
    }
}