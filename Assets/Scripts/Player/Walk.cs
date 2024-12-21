using Unity.VisualScripting;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private int speed;

    private float multiply = 1f;

    public void DoWalk(Vector2 moveAxis)
    {
        Vector3 _move = transform.right * moveAxis.x + transform.forward * moveAxis.y;
        characterController.Move(_move * Time.deltaTime * speed * multiply);
    }

    public void SetMultiply(float value)
    {
        multiply = value;
    }
}
