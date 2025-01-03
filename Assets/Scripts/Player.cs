using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // InputActions
    [Header("Input Actions")]
    [SerializeField]
    private InputAction         IA_crouch;
    [SerializeField]
    private InputAction         IA_jump;
    [SerializeField]
    private InputAction         IA_walk;
    [SerializeField]
    private InputAction         IA_mouseX;
    [SerializeField]
    private InputAction         IA_mouseY;
    [SerializeField]
    private InputAction         IA_zoom;
    [SerializeField]
    private InputAction         IA_run;
    [SerializeField]
    private InputAction         IA_interact;

    // RotateCamera
    [Header("Rotate Camera")]
    [Range(0f, 10f)]
    [SerializeField]
    private float               f_sensetivity;
    [Range(0f, 10f)]
    [SerializeField]
    private float               f_multiplySensetivity;
    private Camera              cam_mainCamera;
    private Transform           t_uiCamera;
    private float               f_xRotation = 0f;
    private float               f_xRotationLimit = 90f;

    // RotatePlayer
    private Transform           t_player;

    // Zoom
    [Header("Zoom")]
    [Range(0f, 1f)]
    [SerializeField]
    private float               multiplyZoom;
    [Range(0f, 10f)]
    [SerializeField]
    private float               zoomDelta;
    private float               f_currentFov;
    private float               f_targetFov;

    // Movement
    [Header("Movement")]
    [SerializeField]
    private float               f_moveSpeed;
    private float               f_crouchSpeed;
    private float               f_runningSpeed;
    private Vector3             moveVelocity;
    private CharacterController cc_player;

    // BOOLS
    private bool                b_isRunning     = false;
    private bool                b_isInAir       = false; // TODO: realize isInAir state
    private bool                b_isCrouching   = false;
    private bool                b_isInteracting = false;

    // Gravity
    [Header("Gravity")]
    [SerializeField]
    private float               f_gravityValue;
    [SerializeField]
    private LayerMask           lm_gravityMask;
    [SerializeField]
    private Transform           t_gravityCheck;
    private Vector3             gravityVelocity;

    // Jump
    [Header("Jump")]
    [SerializeField]
    private float               i_jumpForce;

    // Headbob
    [Header("Headbob")]
    [Range(0.001f, 0.01f)]
    [SerializeField]
    private float               f_bobAmount = 0.002f;
    [Range(1f, 30f)]
    [SerializeField]
    private float               f_bobFreq = 10.0f;
    [Range(10f, 100f)]
    [SerializeField]
    private float               f_bobSmooth = 10.0f;
    private float               f_bobRunFreq;
    private float               f_bobCrouchFreq;
    private Transform           t_headbobPivot;
    private Vector3             bobStartPos;

    // Crouch
    [Header("Crouch")]
    [Range(0, 1f)]
    [SerializeField]
    private float               f_crouchHeight;
    [Range(0, 5f)]
    [SerializeField]
    private float               f_crouchDelta;
    private float               f_normalHeight = 2.0f;
    private Transform           t_cameraPivot;
    private Vector3             colCenter;

    // Raycast
    [Header("Raycast")]
    [Range(0, 10)]
    [SerializeField]
    private int                 i_raycastDistance;

    // Karavan
    [SerializeField] private GameObject karavan;


    /*
     *********
     * START *
     *********
     */

    private void Start()
    {
        // Start-up settings
        Application.targetFrameRate = 300;
        Cursor.lockState            = CursorLockMode.Locked;

        // Init
        InitDependencies();
        CheckDependencies();

        // Headbob
        bobStartPos                 = t_headbobPivot.localPosition;
        f_bobRunFreq                = f_bobFreq * 1.4f;
        f_bobCrouchFreq             = f_bobFreq * 0.6f;

        // Crouch
        colCenter                   = cc_player.center;

        // Zoom
        f_currentFov                = cam_mainCamera.fieldOfView;
        f_targetFov                 = f_currentFov * multiplyZoom;
    }

    /*
     **********
     * UPDATE *
     **********
     */

    private void Update()
    {
        CheckInputs();
        CheckInteractRaycast();
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
     ****************
     * INTERACTABLE *
     ****************
     */

    private void CheckInteractRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam_mainCamera.transform.position, cam_mainCamera.transform.forward, out hit, i_raycastDistance))
        {
            // Door
            if (hit.collider.tag == "Door")
            {
                if (IA_interact.triggered && !b_isInteracting)
                {
                    b_isInteracting = true;

                    Door door = hit.collider.GetComponent<Door>();
                    if (door.IsOpened())
                    {
                        door.CloseDoor();
                    } else
                    {
                        door.OpenDoor();
                    }
                } 
            }

            // Uncheck interact bool key-press
            if (IA_interact.ReadValue<float>() == 0)
            {
                b_isInteracting = false;
            }
        }
    }

    /*
     ***********
     * HEADBOB *
     ***********
     */
    
    private Vector3 StartHeadBob()
    {
        // TODO: (HEADBOB) add state dependencies
        Vector3 pos = Vector3.zero;

        if (b_isRunning && !b_isCrouching)
        {
            
            pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * f_bobRunFreq) * f_bobAmount * 1.4f, f_bobSmooth * Time.deltaTime);
            pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * f_bobRunFreq / 2) * f_bobAmount * 1.6f, f_bobSmooth * Time.deltaTime);
            t_headbobPivot.localPosition += pos;
        }
        else if (b_isCrouching && !b_isRunning)
        {
            pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * f_bobCrouchFreq) * f_bobAmount * 1.4f, f_bobSmooth * Time.deltaTime);
            pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * f_bobCrouchFreq / 2) * f_bobAmount * 1.6f, f_bobSmooth * Time.deltaTime);
            t_headbobPivot.localPosition += pos;
        } else
        {
            pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * f_bobFreq) * f_bobAmount * 1.4f, f_bobSmooth * Time.deltaTime);
            pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * f_bobFreq / 2) * f_bobAmount * 1.6f, f_bobSmooth * Time.deltaTime);
            t_headbobPivot.localPosition += pos;
        }

        return pos;
    }

    private void StopHeadbob()
    {
        if (t_headbobPivot.localPosition == bobStartPos) return;
        t_headbobPivot.localPosition = Vector3.Lerp(t_headbobPivot.localPosition, bobStartPos, 7 * Time.deltaTime);
    }

    /*
     **********
     * CROUCH *
     **********
     */

    private void StartCrouch()
    {
        cc_player.height = Mathf.Lerp(cc_player.height, f_crouchHeight, f_crouchDelta * Time.deltaTime);

        float f_centerY = (cc_player.height / 2) - (f_normalHeight / 2);
        cc_player.center = new Vector3(colCenter.x, f_centerY, colCenter.z);

        Vector3 cameraOffset = new Vector3(0, cc_player.height / 2 - 0.5f, 0);
        t_cameraPivot.localPosition = cameraOffset;
    }

    private void StopCrouch()
    {
        cc_player.height = Mathf.Lerp(cc_player.height, f_normalHeight, f_crouchDelta * Time.deltaTime);

        float f_centerY = (cc_player.height / 2) - (f_normalHeight / 2);
        cc_player.center = new Vector3(colCenter.x, f_centerY, colCenter.z);

        Vector3 cameraOffset = new Vector3(0, cc_player.height / 2 - 0.5f, 0);
        t_cameraPivot.localPosition = cameraOffset;
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
        return Physics.CheckSphere(t_gravityCheck.position, 0.4f, lm_gravityMask);
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
        f_xRotation  = Mathf.Clamp(f_xRotation, -f_xRotationLimit, f_xRotationLimit);

        cam_mainCamera.transform.localRotation = Quaternion.Euler(f_xRotation, 0f, 0f);
        t_uiCamera.localRotation               = Quaternion.Euler(f_xRotation, 0f, 0f);
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
        IA_interact.Enable();
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
        IA_interact.Disable();
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
        t_uiCamera     = GameObject.Find("UICamera").GetComponent<Transform>();
        t_player       = gameObject.GetComponent<Transform>();
        cc_player      = gameObject.GetComponent<CharacterController>();
        t_cameraPivot  = GameObject.Find("CameraHolder").GetComponent<Transform>();
        t_headbobPivot = GameObject.Find("HeadBobHolder").GetComponent<Transform>();
    }

    private void CheckDependencies()
    {
        if (cam_mainCamera == null) { Debug.LogError("cam_mainCamera is NULL", this); }
        if (t_uiCamera     == null) { Debug.LogError("t_uiCamera is NULL", this); }
        if (t_player       == null) { Debug.LogError("t_player is NULL", this); }
        if (cc_player      == null) { Debug.LogError("cc_player is NULL", this); }
        if (t_cameraPivot  == null) { Debug.LogError("t_cameraPivot is NULL", this); }
        if (t_headbobPivot == null) { Debug.LogError("t_headbobPivot is NULL", this); }
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
