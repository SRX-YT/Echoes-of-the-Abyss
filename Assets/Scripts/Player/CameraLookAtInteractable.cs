using UnityEngine;
using UnityEngine.UI;

public class CameraLookAtInteractable : MonoBehaviour
{
    private Outline outline;

    private void InitDependencies()
    {
        outline = GetComponent<Outline>();
    }

    private void CheckDependencies()
    {
        if (outline == null) { Debug.LogError("outline is NULL!"); }
    }
}
