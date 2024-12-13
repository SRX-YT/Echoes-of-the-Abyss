using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private Transform playerT;

    [Header("Settings")]
    [SerializeField]
    private float mouseSensetivity;
    [SerializeField]
    private float sensetivityMultiply;

    private void Start()
    {
        InitDependencies();
        CheckDependencies();
    }

    public void Rotate(float value)
    {
        value = value * mouseSensetivity * Time.deltaTime * sensetivityMultiply;

        playerT.Rotate(Vector3.up * value);
    }

    private void InitDependencies()
    {
        playerT = GetComponent<Transform>();
    }

    private void CheckDependencies()
    {
        if (playerT == null) { Debug.LogError("playerT is NULL!"); }
    }
}
