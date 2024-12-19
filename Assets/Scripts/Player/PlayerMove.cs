using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController characterController;

    private Vector3 playerVelocity;

    private float gravityValue = -9.81f;

    [Header("Settings")]
    [SerializeField] private int playerSpeed;

    private void Start()
    {
        InitDependencies();
        CheckDependencies();
    }

    private void Update()
    {
        Gravity();
    }

    public void Move(Vector2 moveAxis)
    {
        Vector3 _move = transform.right * moveAxis.x + transform.forward * moveAxis.y;
        characterController.Move(_move * Time.deltaTime * playerSpeed);
    }

    private void Gravity()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void InitDependencies()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void CheckDependencies()
    {
        if (characterController == null) { Debug.LogError("characterController is NULL!"); }
    }
}
