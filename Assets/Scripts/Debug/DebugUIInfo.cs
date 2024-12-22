using TMPro;
using UnityEngine;

public class DebugUIInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private WorldManager worldManager;
    private float hour, minute;
    private float currentMinute;

    private void Update()
    {
        velocityText.text = characterController.velocity.ToString();
        speedText.text = characterController.velocity.magnitude.ToString();

        hour = worldManager.GetDayDuration() / 24;
        minute = hour / 60;
        currentMinute = (int)((worldManager.GetTime() / minute) - (int)(worldManager.GetTime() / hour) * 60);

        timeText.text = "Time: " + ((int)(worldManager.GetTime() / hour)).ToString() + ":" + currentMinute.ToString();
    }
}
