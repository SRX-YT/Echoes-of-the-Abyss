using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // InputActions
    [Header("Input Actions")]
    [SerializeField] private InputAction IA_crouch;  // TODO
    [SerializeField] private InputAction IA_jump;    // TODO
    [SerializeField] private InputAction IA_walk;    // TODO
    [SerializeField] private InputAction IA_mouseX;
    [SerializeField] private InputAction IA_mouseY;
    [SerializeField] private InputAction IA_zoom;

    // RotateCamera
    [Header("Rotate Camera")]
    [Range(0f, 10f)]
    [SerializeField] private float sensetivity;
    [Range(0f, 10f)]
    [SerializeField] private float multiplySensetivity;
    private Camera cam_mainCamera;
    private Transform t_uiCamera;
    private float xRotation = 0f;
    private float xRotationLimit = 90f;

    // RotatePlayer
    private Transform t_player;

    // Zoom
    [Header("Zoom")]
    [Range(0f, 1f)]
    [SerializeField] private float multiplyZoom;
    [Range(0f, 10f)]
    [SerializeField] private float zoomDelta;
    private float currentFov;
    private float targetFov;

    // Movement
    [Header("Movement")]
    

    // Gravity
    [Header("Gravity")]
    [Range(0f, 1f)]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    private Transform t_groundCheck; // INIT


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

        // Zoom
        currentFov = cam_mainCamera.fieldOfView;
        targetFov = currentFov * multiplyZoom;
    }

    /*
     **********
     * UPDATE *
     **********
     */

    private void Update()
    {
        CheckInputs();
        DebugAll();
    }

    /*
     **********
     * CHECKS *
     **********
     */

    private void CheckInputs()
    {
        float mouseX = IA_mouseX.ReadValue<float>();
        if (mouseX != 0f) { DoRotatePlayer(mouseX); }

        float mouseY = IA_mouseY.ReadValue<float>();
        if (mouseY != 0f) { DoRotateCamera(mouseY); }

        if (IA_zoom.ReadValue<float>() != 0f) { DoZoom(); }
        else { StopZoom(); }

        Vector2 axis = IA_walk.ReadValue<Vector2>();
        if (axis != Vector2.zero) {}
    }

    /*
     ************
     * MOVEMENT *
     ************
     */

    

    /*
     ***********
     * GRAVITY *
     ***********
     */

    private void Gravity()
    {

    }

    private bool IsGrounded()
    {
        return Physics.Raycast(t_groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    /*
     **********
     * ROTATE *
     **********
     */

    private void DoRotateCamera(float value)
    {
        value = value * sensetivity * Time.deltaTime * multiplySensetivity;
        xRotation -= value;
        xRotation = Mathf.Clamp(xRotation, -xRotationLimit, xRotationLimit);
        cam_mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        t_uiCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void DoRotatePlayer(float value)
    {
        value = value * sensetivity * Time.deltaTime * multiplySensetivity;
        t_player.Rotate(Vector3.up * value);
    }

    /*
     ********
     * ZOOM *
     ********
     */

    private void DoZoom()
    {
        cam_mainCamera.fieldOfView = Mathf.Lerp(cam_mainCamera.fieldOfView, targetFov, zoomDelta * Time.deltaTime);
    }

    private void StopZoom()
    {
        cam_mainCamera.fieldOfView = Mathf.Lerp(cam_mainCamera.fieldOfView, currentFov, zoomDelta * Time.deltaTime);
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
    }

    private void OnDisable()
    {
        IA_crouch.Disable();
        IA_jump.Disable();
        IA_walk.Disable();
        IA_mouseX.Disable();
        IA_mouseY.Disable();
        IA_zoom.Disable();
    }

    /*
     *********
     * DEBUG *
     *********
     */

    private void DebugAll()
    {
        Debug.DrawRay(t_groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
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
        t_groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
    }

    private void CheckDependencies()
    {
        if (cam_mainCamera == null) { Debug.LogError("cam_mainCamera is NULL", this); }
        if (t_uiCamera == null) { Debug.LogError("t_uiCamera is NULL", this); }
        if (t_player == null) { Debug.LogError("t_player is NULL", this); }
        if (t_groundCheck == null) { Debug.LogError("t_groundCheck is NULL", this); }
    }
}
