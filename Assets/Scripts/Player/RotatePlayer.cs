using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    [SerializeField] private float sensetivity;
    [SerializeField] private float multiply;
    [SerializeField] Transform player;

    public void DoRotate(float value)
    {
        value = value * sensetivity * Time.deltaTime * multiply;
        player.Rotate(Vector3.up * value);
    }
}
