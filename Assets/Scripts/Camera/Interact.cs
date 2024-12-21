using UnityEngine;

public class Interact : MonoBehaviour
{
    private string targetTag = "Interactable";
    private Outline outline;

    [SerializeField] private int maxDistanceRaycast = 5;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        CheckRaycast();
    }

    private void CheckRaycast()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistanceRaycast, layerMask))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                if (outline == null)
                {
                    outline = hit.collider.gameObject.AddComponent<Outline>();
                    outline.OutlineWidth = 10;
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                }
            } else
            {
                DestroyTrash();
            }
        } else
        {
            DestroyTrash();
        }
    }

    private void DestroyTrash()
    {
        Destroy(outline);
    }
}
