using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // InputActions
    [Header("Input Actions")]
    [SerializeField] private InputAction IA_crouch;
    [SerializeField] private InputAction IA_jump;
    [SerializeField] private InputAction IA_walk;
    [SerializeField] private InputAction IA_mouseX;
    [SerializeField] private InputAction IA_mouseY;
    [SerializeField] private InputAction IA_zoom;
    [SerializeField] private InputAction IA_run;

    // RotateCamera
    [Header("Rotate Camera")]
    [Range(0f, 10f)]
    [SerializeField] private float f_sensetivity;
    [Range(0f, 10f)]
    [SerializeField] private float f_multiplySensetivity;
    private Camera cam_mainCamera;
    private Transform t_uiCamera;
    private float f_xRotation = 0f;
    private float f_xRotationLimit = 90f;

    // RotatePlayer
    private Transform t_player;

    // Zoom
    [Header("Zoom")]
    [Range(0f, 1f)]
    [SerializeField] private float multiplyZoom;
    [Range(0f, 10f)]
    [SerializeField] private float zoomDelta;
    private float f_currentFov;
    private float f_targetFov;

    // Movement
    [Header("Movement")]
    [SerializeField] private float f_moveSpeed;
    private float f_crouchSpeed;
    private float f_runningSpeed;
    private Vector3 moveVelocity;
    private CharacterController cc_player;

    // BOOLS
    private bool b_isRunning = false;
    private bool b_isInAir = false; // TODO: realize isInAir state
    private bool b_isCrouching = false;

    // Gravity
    [Header("Gravity")]
    [SerializeField] private float f_gravityValue;
    private Vector3 gravityVelocity;

    // Jump
    [Header("Jump")]
    [SerializeField] private float i_jumpForce;

    // Headbob
    [Header("Headbob")]
    [Range(0.001f, 0.01f)]
    [SerializeField] private float f_bobAmount = 0.002f;
    [Range(1f, 30f)]
    [SerializeField] private float f_bobFreq = 10.0f;
    [Range(10f, 100f)]
    [SerializeField] private float f_bobSmooth = 10.0f;
    private Transform t_cameraPivot;
    private Vector3 bobStartPos;


    /*
     *********
     * START *
     *********
     */

    private void Start()
    {
        // Start-up settings
        Application.targetFrameRate = 300;
        Cursor.lockState = CursorLockMode.Locked;

        // Init
        InitDependencies();
        CheckDependencies();

        // Headbob
        bobStartPos = t_cameraPivot.localPosition;

        // Zoom
        f_currentFov = cam_mainCamera.fieldOfView;
        f_targetFov = f_currentFov * multiplyZoom;
    }

    /*
     **********
     * UPDATE *
     **********
     */

    private void Update()
    {
        CheckInputs();
        Gravity();
        DebugAll();
    }

    /*
     **********
     * CHECKS *
     **********
     */

    private void CheckInputs()
    {
        // Rotate player
        float mouseX = IA_mouseX.ReadValue<float>();
        if (mouseX != 0f)
        { 
            DoRotatePlayer(mouseX);
        }

        // Rotate camera
        float mouseY = IA_mouseY.ReadValue<float>();
        if (mouseY != 0f)
        { 
            DoRotateCamera(mouseY);
        }

        // Zoom
        if (IA_zoom.ReadValue<float>() != 0f)
        { 
            DoZoom();
        }
        else
        { 
            StopZoom();
        }

        // Walk, Headbob
        Vector2 axis = IA_walk.ReadValue<Vector2>();
        if (axis != Vector2.zero)
        { 
            StartMove(axis);
            StartHeadBob();
        }
        else
        { 
            StopMove();
            StopHeadbob();
        }

        // Jump
        if (IA_jump.IsPressed())
        { 
            DoJump();
        }

        // Run
        if (IA_run.ReadValue<float>() != 0f)
        { 
            b_isRunning = true;
        }
        else
        { 
            b_isRunning = false;
        }

        // Crouch
        if (IA_crouch.ReadValue<float>() != 0f)
        {
            StartCrouch();
            b_isCrouching = true;
        }
        else
        {
            StopCrouch();
            b_isCrouching = false;
        }
    }

    /*
     ************
     * MOVEMENT *
     ************
     */

    private void StartMove(Vector2 axis)
    {
        moveVelocity = transform.forward * axis.y + transform.right * axis.x;
        if (b_isRunning && !b_isCrouching)
        {
            f_runningSpeed = f_moveSpeed * 1.5f;
            cc_player.Move(moveVelocity * Time.deltaTime * f_runningSpeed);
        }
        else if (b_isCrouching)
        {
            f_crouchSpeed = f_moveSpeed / 2;
            cc_player.Move(moveVelocity * Time.deltaTime * f_crouchSpeed);
        }
        else
        {
            cc_player.Move(moveVelocity * Time.deltaTime * f_moveSpeed);
        }
    }

    private void StopMove()
    {
        moveVelocity = Vector3.zero;
        if (moveVelocity != Vector3.zero)
        {
            cc_player.Move(moveVelocity * Time.deltaTime * f_moveSpeed);
        }
    }

    /*
     ***********
     * HEADBOB *
     ***********
     */
    
    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;

        // TODO: (HEADBOB) add state dependencies
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * f_bobFreq) * f_bobAmount * 1.4f, f_bobSmooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * f_bobFreq / 2) * f_bobAmount * 1.6f, f_bobSmooth * Time.deltaTime);
        t_cameraPivot.localPosition += pos;

        return pos;
    }

    private void StopHeadbob()
    {
        if (t_cameraPivot.localPosition == bobStartPos) return;
        t_cameraPivot.localPosition = Vector3.Lerp(t_cameraPivot.localPosition, bobStartPos, 5 * Time.deltaTime);
    }

    /*
     **********
     * CROUCH *
     **********
     */

    private void StartCrouch()
    {
        cc_player.height = 1;
    }

    private void StopCrouch()
    {
        cc_player.height = 2;
    }

    /*
     ***********
     * GRAVITY *
     ***********
     */

    private void Gravity()
    {
        if (IsGrounded() && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -3f;
        }
        else
        {
            gravityVelocity.y += f_gravityValue * Time.deltaTime;
            b_isInAir = true;
        }
        cc_player.Move(gravityVelocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        // TODO: (ISGROUNDED) change raycast to sphere
        return Physics.Raycast(transform.position, Vector3.down, (cc_player.height / 2) + 0.4f);
    }

    /*
     ********
     * JUMP *
     ********
     */

    private void DoJump()
    {
        if (IsGrounded())
        {
            gravityVelocity.y = Mathf.Sqrt(i_jumpForce * -2f * f_gravityValue);
        }
    }

    /*
     **********
     * ROTATE *
     **********
     */

    private void DoRotateCamera(float value)
    {
        value = value * f_sensetivity * 0.01f * f_multiplySensetivity;

        f_xRotation -= value;
        f_xRotation = Mathf.Clamp(f_xRotation, -f_xRotationLimit, f_xRotationLimit);

        cam_mainCamera.transform.localRotation = Quaternion.Euler(f_xRotation, 0f, 0f);
        t_uiCamera.localRotation = Quaternion.Euler(f_xRotation, 0f, 0f);
    }

    private void DoRotatePlayer(float value)
    {
        value = value * f_sensetivity * 0.01f * f_multiplySensetivity;
        gameObject.transform.Rotate(0, value, 0);
    }

    /*
     ********
     * ZOOM *
     ********
     */

    private void DoZoom()
    {
        cam_mainCamera.fieldOfView = Mathf.Lerp(cam_mainCamera.fieldOfView, f_targetFov, zoomDelta * Time.deltaTime);
    }

    private void StopZoom()
    {
        cam_mainCamera.fieldOfView = Mathf.Lerp(cam_mainCamera.fieldOfView, f_currentFov, zoomDelta * Time.deltaTime);
    }

    /*
     *********
     * OTHER *
     *********
     */

    private void OnEnable()
    {
        IA_crouch.Enable();
        IA_jump.Enable();
        IA_walk.Enable();
        IA_mouseX.Enable();
        IA_mouseY.Enable();
        IA_zoom.Enable();
        IA_run.Enable();
    }

    private void OnDisable()
    {
        IA_crouch.Disable();
        IA_jump.Disable();
        IA_walk.Disable();
        IA_mouseX.Disable();
        IA_mouseY.Disable();
        IA_zoom.Disable();
        IA_run.Disable();
    }

    /*
     *********
     * DEBUG *
     *********
     */

    private void DebugAll()
    {
        
    }

    /*
     ********
     * INIT *
     ********
     */

    private void InitDependencies()
    {
        cam_mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        t_uiCamera = GameObject.Find("UICamera").GetComponent<Transform>();
        t_player = gameObject.GetComponent<Transform>();
        cc_player = gameObject.GetComponent<CharacterController>();
        t_cameraPivot = GameObject.Find("CameraHolder").GetComponent<Transform>();
    }

    private void CheckDependencies()
    {
        if (cam_mainCamera == null) { Debug.LogError("cam_mainCamera is NULL", this); }
        if (t_uiCamera == null) { Debug.LogError("t_uiCamera is NULL", this); }
        if (t_player == null) { Debug.LogError("t_player is NULL", this); }
        if (cc_player == null) { Debug.LogError("cc_player is NULL", this); }
        if (t_cameraPivot == null) { Debug.LogError("t_cameraPivot is NULL", this); }
    }

    /*
     ***********
     * GETTERS *
     ***********
     */

    public Vector3 GetVelocity()
    {
        return moveVelocity + gravityVelocity;
    }
}
