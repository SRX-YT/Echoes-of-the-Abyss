using TMPro;
using UnityEngine;

public class DebugUIInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] private CharacterController characterController;

    private void Update()
    {
        velocityText.text = characterController.velocity.ToString();
        speedText.text = characterController.velocity.magnitude.ToString();
    }
}
