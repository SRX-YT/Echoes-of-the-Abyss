using TMPro;
using UnityEngine;

public class DebugUIInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fpsText;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private WorldManager worldManager;
    private float hour, minute;
    private float currentMinute;
    private float deltaTime; // fps

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " : Fps";

        hour = worldManager.GetDayDuration() / 24;
        minute = hour / 60;
        currentMinute = (int)((worldManager.GetTime() / minute) - (int)(worldManager.GetTime() / hour) * 60);

        timeText.text = "Time: " + ((int)(worldManager.GetTime() / hour)).ToString() + ":" + currentMinute.ToString();
    }
}
