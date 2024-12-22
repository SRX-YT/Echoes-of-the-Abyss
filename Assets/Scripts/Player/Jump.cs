using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Gravity gravity;
    [Range(0f, 10f)]
    [SerializeField] private int jumpForce;

    public void DoJump()
    {
        if (characterController.isGrounded)
        {
            gravity.SetVelocity(Mathf.Sqrt(jumpForce * -2f * -9.81f));
        }
    }
}
