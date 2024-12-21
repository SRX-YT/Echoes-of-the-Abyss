using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraLookAtInteractable : MonoBehaviour
{
    // Raycast
    private string targetTag = "Interactable";
    private int maxDistanceCast = 5;
    private Camera mainCamera;

    // Outline component
    private Outline outline;

    // UI interactable
    private GameObject interactableChoisePanelUI;

    [Header("Settings")]
    [SerializeField] private LayerMask layerMask;

    private void Start()
    {
        InitDependencies();
        CheckDependencies();
    }

    private void Update()
    {
        Ray _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, maxDistanceCast, layerMask))
        {
            if (_hit.collider.CompareTag(targetTag))
            {
                if (outline == null)
                { 
                    _hit.collider.AddComponent<Outline>(); 
                    outline = _hit.collider.GetComponent<Outline>();
                    outline.OutlineWidth = 3;
                }

                interactableChoisePanelUI.SetActive(true);
            }
            else
            {
                DestroyTrash();
            }
        }
        else
        {
            DestroyTrash();
        }
    }

    private void DestroyTrash()
    {
        if (outline != null) { Destroy(outline); }

        interactableChoisePanelUI.SetActive(false);
    }

    private void InitDependencies()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        interactableChoisePanelUI = GameObject.Find("InteractableChoisePanel");
        interactableChoisePanelUI.SetActive(false);
    }

    private void CheckDependencies()
    {
        if (mainCamera == null) { Debug.LogError("mainCamera is NULL!"); }
        if (interactableChoisePanelUI == null) { Debug.LogError("interactableChoisePanelUI is NULL!"); }
    }
}
