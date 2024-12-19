using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerPlayer : MonoBehaviour
{
    private PlayerRotate playerRotate;
    private PlayerMove playerMove;

    private CameraRotate cameraRotate;

    [Header("Player Controls Settings")]
    [SerializeField] private InputAction playerMovementIA;
    [SerializeField] private InputAction playerRotateIA;
    [SerializeField] private InputAction cameraRotateIA;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        InitDependencies();
        CheckDependencies();
    }

    private void Update()
    {
        CheckPlayerRotate();
        CheckPlayerMove();
        CheckCameraRotate();
    }

    private void CheckPlayerRotate()
    {
        float _axis = playerRotateIA.ReadValue<float>();
        if (_axis != 0)
        {
            playerRotate.Rotate(_axis);
        }
    }

    private void CheckPlayerMove()
    {
        Vector2 _moveAxis = playerMovementIA.ReadValue<Vector2>();
        if (_moveAxis != Vector2.zero)
        {
            playerMove.Move(_moveAxis);
        }
    }

    private void CheckCameraRotate()
    {
        float _axis = cameraRotateIA.ReadValue<float>();
        if (_axis != 0)
        {
            cameraRotate.Rotate(_axis);
        }
    }

    private void InitDependencies()
    {
        // Temp playerGO for better performance while searching dependencies
        GameObject playerGO = GameObject.Find("Player");

        playerRotate = playerGO.GetComponent<PlayerRotate>();
        playerMove = playerGO.GetComponent<PlayerMove>();
        cameraRotate = playerGO.GetComponent<CameraRotate>();
    }

    private void CheckDependencies()
    {
        if (playerRotate == null) { Debug.LogError("playerRotate is NULL!"); }
        if (playerMove == null) { Debug.LogError("playerMove is NULL!"); }
        if (cameraRotate == null) { Debug.LogError("cameraRotate is NULL!"); }
    }

    private void OnEnable()
    {
        playerMovementIA.Enable();
        playerRotateIA.Enable();
        cameraRotateIA.Enable();
    }

    private void OnDisable()
    {
        playerMovementIA.Disable();
        playerRotateIA.Disable();
        cameraRotateIA.Disable();
    }
}
