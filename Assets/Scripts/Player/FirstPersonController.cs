using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    public UserSettings userSettings;
    public PlatformManager platformManager;
    public Joystick joystick;
    public TouchLookArea touchLook;

    public Transform playerCamera;
    public Transform cameraParent;
    public Transform groundCheck;

    [Header("Look")]
    public float minPitch = -70f;
    public float maxPitch = 70f;
    public float snappiness = 100f;
    public float touchSensitivity = 5f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float jumpForce = 3f;
    public float gravity = 9.81f;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float crouchCameraHeight = 1f;
    public float crouchSpeedMultiplier = 0.5f;

    [Header("Ground Check")]
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController controller;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;

    private bool jumpPressed;
    private bool crouchHeld;



    private float rotX, rotY;
    private float xVel, yVel;

    private float originalHeight;
    private float originalCamY;
    private float currentCamY;
    private float camVel;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        originalHeight = controller.height;
        originalCamY = cameraParent.localPosition.y;
        currentCamY = originalCamY;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleGroundCheck();
        HandleMovement();
        HandleCrouch();
    }

    // ================= LOOK =================
    void HandleLook()
    {
        float inputX = 0f;
        float inputY = 0f;

        if (platformManager.isPC)
        {
            inputX = Input.GetAxis(userSettings.mouseXAxis);
            inputY = Input.GetAxis(userSettings.mouseYAxis);

            if (userSettings.invertMouseY)
                inputY *= -1f;
        }
        else if (platformManager.isAndroid)
        {
            Vector2 touchInput = touchLook.GetInput();
            inputX = touchInput.x * touchSensitivity;
            inputY = touchInput.y * touchSensitivity;
        }

        rotX += inputX * userSettings.mouseSensitivity * Time.deltaTime;
        rotY -= inputY * userSettings.mouseSensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, minPitch, maxPitch);

        xVel = Mathf.Lerp(xVel, rotX, snappiness * Time.deltaTime);
        yVel = Mathf.Lerp(yVel, rotY, snappiness * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, xVel, 0f);
        playerCamera.localRotation = Quaternion.Euler(yVel, 0f, 0f);
    }

    // ================= GROUND =================
    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    // ================= MOVEMENT =================
    void HandleMovement()
    {
        float x = 0f;
        float z = 0f;

        if (platformManager.isAndroid)
        {
            x = joystick.Horizontal();
            z = joystick.Vertical();
        }
        else if (platformManager.isPC)
        {
            if (Input.GetKey(userSettings.move_Left)) x -= 1f;
            if (Input.GetKey(userSettings.move_Right)) x += 1f;
            if (Input.GetKey(userSettings.move_Forward)) z += 1f;
            if (Input.GetKey(userSettings.move_Backward)) z -= 1f;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        float speed = isCrouching ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        controller.Move(move * speed * Time.deltaTime);

        if (platformManager.isPC)
        {
            if (Input.GetKey(userSettings.move_Jump) && isGrounded)
                velocity.y = jumpForce;
        }
        else if (platformManager.isAndroid)
        {
            if (jumpPressed && isGrounded)
            {
                velocity.y = jumpForce;
            }
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ================= CROUCH =================
    void HandleCrouch()
    {
        if (platformManager.isPC)
        {
            isCrouching = Input.GetKey(userSettings.move_Crouch);
        }
        else if (platformManager.isAndroid)
        {
            isCrouching = crouchHeld;
        }

        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);
        controller.center = new Vector3(0, controller.height / 2f, 0);

        float targetCamY = isCrouching ? crouchCameraHeight : originalCamY;

        currentCamY = Mathf.SmoothDamp(
            currentCamY,
            targetCamY,
            ref camVel,
            0.1f
        );

        cameraParent.localPosition = new Vector3(0, currentCamY, 0);
    }    
    // EventTrigger jump and crouch 
    public void JumpButtonDown() => jumpPressed = true;
    public void JumpButtonUp() => jumpPressed = false;
    public void CrouchButtonDown() => crouchHeld = true;
    public void CrouchButtonUp() => crouchHeld = false;
}