using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [Range(0.5f, 1f)]
    [SerializeField] private float multiply;
    [Range(0f, 10f)]
    [SerializeField] private float zoomDelta;

    private float currentFov;
    private float targetFov;

    private void Start()
    {
        currentFov = mainCamera.fieldOfView;
        targetFov = currentFov * multiply;
    }

    public void DoZoom()
    {
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFov, zoomDelta * Time.deltaTime); 
    }

    public void StopZoom()
    {
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, currentFov, zoomDelta * Time.deltaTime);
    }
}
