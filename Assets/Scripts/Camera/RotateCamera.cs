using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform uiCamera;
    [SerializeField] private float sensetivity;
    [SerializeField] private float multiply;

    private float xRotation = 0f;
    private float xRotationLimit = 90f;

    public void DoRotate(float value)
    {
        value = value * sensetivity * Time.deltaTime * multiply;

        xRotation -= value;
        xRotation = Mathf.Clamp(xRotation, -xRotationLimit, xRotationLimit);

        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        uiCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
