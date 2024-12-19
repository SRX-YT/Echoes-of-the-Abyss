using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private Transform cameraHolderT;
    private Transform cameraUIHolderT;

    private float xRotation = 0f;
    private float xRotationLimit = 90f;

    [Header("Settings")]
    [SerializeField] private float mouseSensetivity;
    [SerializeField] private float sensetivityMultiply;

    private void Start()
    {
        InitDependencies();
        CheckDependencies();
    }

    public void Rotate(float value)
    {
        value = value * mouseSensetivity * Time.deltaTime * sensetivityMultiply;

        xRotation -= value;
        xRotation = Mathf.Clamp(xRotation, -xRotationLimit, xRotationLimit);
        
        cameraHolderT.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        cameraUIHolderT.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void InitDependencies()
    {
        cameraHolderT = GameObject.Find("MainCamera").GetComponent<Transform>();
        cameraUIHolderT = GameObject.Find("UICamera").GetComponent<Transform>();
    }

    private void CheckDependencies()
    {
        if (cameraHolderT == null) { Debug.LogError("cameraHolderT is NULL!"); }
        if (cameraUIHolderT == null) { Debug.LogError("cameraUIHolderT is NULL!"); }
    }
}
