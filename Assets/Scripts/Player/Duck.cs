using UnityEngine;

public class Duck : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraHolder;

    private float normalHeight = 2.0f;

    private Vector3 colCenter;

    [Range(0f, 1f)]
    [SerializeField] private float crouchHeight;
    
    [Range(0f, 5f)]
    [SerializeField] private float courchDelta;

    private void Start()
    {
        colCenter = characterController.center;
    }

    public void DoDuck()
    {
        characterController.height = Mathf.Lerp(characterController.height, crouchHeight, courchDelta * Time.deltaTime);

        float centerY = (characterController.height / 2) - (normalHeight / 2);
        characterController.center = new Vector3(colCenter.x, centerY, colCenter.z);

        Vector3 cameraOffset = new Vector3(0, characterController.height / 2 - 0.5f, 0);
        cameraHolder.localPosition = cameraOffset;
    }

    public void StopDuck()
    {
        characterController.height = Mathf.Lerp(characterController.height, normalHeight, courchDelta * Time.deltaTime);

        float centerY = (characterController.height / 2) - (normalHeight / 2);
        characterController.center = new Vector3(colCenter.x, centerY, colCenter.z);

        Vector3 cameraOffset = new Vector3(0, characterController.height / 2 - 0.5f, 0);
        cameraHolder.localPosition = cameraOffset;
    }
}
