using UnityEngine;

public class UserSettings : MonoBehaviour
{
    [Header("=== KEY BINDINGS ===")]
    public KeyCode main_Action = KeyCode.Mouse0;
    public KeyCode second_Action = KeyCode.Mouse1;
    public KeyCode reload_Action = KeyCode.R;
    public KeyCode switch_Action = KeyCode.F;

    [Header("Movement Keys")]
    public KeyCode move_Forward = KeyCode.W;
    public KeyCode move_Backward = KeyCode.S;
    public KeyCode move_Left = KeyCode.A;
    public KeyCode move_Right = KeyCode.D;
    public KeyCode move_Jump = KeyCode.Space;
    public KeyCode move_Crouch = KeyCode.LeftControl; 

    [Header("Mouse Axes")]
    public string mouseXAxis = "Mouse X";
    public string mouseYAxis = "Mouse Y";

    [Header("Movement Axes")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    [Header("Mouse Settings")]
    [Range(0f, 100f)] public float mouseSensitivity = 50f;
    public bool invertMouseY = false;
}
