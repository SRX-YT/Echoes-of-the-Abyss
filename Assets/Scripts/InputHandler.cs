using UnityEngine;
using UnityEngine.InputSystem;

class InputHandler : MonoBehaviour
{
    // PlayerIA
    [SerializeField] private InputAction duckIA;
    [SerializeField] private InputAction jumpIA;
    [SerializeField] private InputAction runIA;
    [SerializeField] private InputAction walkIA;
    [SerializeField] private InputAction useIA;
    [SerializeField] private InputAction mouseXIA;

    // CameraIA
    [SerializeField] private InputAction mouseYIA;
    [SerializeField] private InputAction zoomIA;

    // Player
    [SerializeField] private Walk walk;
    private Vector2 walkValue;
    [SerializeField] private RotatePlayer rotatePlayer;
    private float rotatePlayerValue;
    [SerializeField] private UseThing useThing;
    [SerializeField] private Jump jump;
    [SerializeField] private Run run;
    [SerializeField] private Duck duck;

    // Player + Submarine
    [SerializeField] private Swim swim;

    // Camera
    [SerializeField] private RotateCamera rotateCamera;
    private float rotateCameraValue;
    [SerializeField] private Interact interact;
    [SerializeField] private Zoom zoom;

    private void Update()
    {
        CheckWalk();
        CheckRotatePlayer();
        CheckRotateCamera();
        CheckRun();
        CheckZoom();
        CheckDuck();
    }

    private void CheckWalk()
    {
        walkValue = walkIA.ReadValue<Vector2>();
        if (walkValue != Vector2.zero)
        {
            walk.DoWalk(walkValue);
        }
    }

    private void CheckRotatePlayer()
    {
        rotatePlayerValue = mouseXIA.ReadValue<float>();
        if (rotatePlayerValue != 0)
        {
            rotatePlayer.DoRotate(rotatePlayerValue);
        }
    }

    private void CheckUseThing()
    {

    }

    private void CheckSwim()
    {

    }

    private void CheckRun()
    {
        if (runIA.ReadValue<float>() != 0)
        {
            run.DoRun();
        } else
        {
            walk.SetMultiply(1);
        }
    }

    private void CheckDuck()
    {
        if (duckIA.ReadValue<float>() != 0)
        {
            duck.DoDuck();
        } else
        {
            duck.StopDuck();
        }
    }

    private void CheckRotateCamera()
    {
        rotateCameraValue = mouseYIA.ReadValue<float>();
        if (rotateCameraValue != 0)
        {
            rotateCamera.DoRotate(rotateCameraValue);
        }
    }

    private void CheckZoom()
    {
        if (zoomIA.ReadValue<float>() > 0)
        {
            zoom.DoZoom();
        } else
        {
            zoom.StopZoom();
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        duckIA.Enable();
        jumpIA.Enable();
        runIA.Enable();
        walkIA.Enable();
        useIA.Enable();
        mouseXIA.Enable();
        mouseYIA.Enable();
        zoomIA.Enable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        duckIA.Disable();
        jumpIA.Disable();
        runIA.Disable();
        walkIA.Disable();
        useIA.Disable();
        mouseXIA.Disable();
        mouseYIA.Disable();
        zoomIA.Disable();
    }
}