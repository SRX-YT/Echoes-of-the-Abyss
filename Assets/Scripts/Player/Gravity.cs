using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] float value;

    private Vector3 velocity;

    private void Update()
    {
        DoGravity();
    }

    private void DoGravity()
    {
        if (characterController.isGrounded && velocity.y < 0) { velocity.y = 0; }

        velocity.y += value * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
