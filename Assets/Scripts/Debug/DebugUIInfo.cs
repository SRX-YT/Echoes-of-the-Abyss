using TMPro;
using UnityEngine;

public class DebugUIInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] private GameObject controlsPanel;
    private bool b_controlsIsOpen = false;

    [SerializeField] private WorldManager worldManager;
    private float hour, minute;
    private float currentMinute;
    private float deltaTime; // fps

    [SerializeField] private Player player;

    private void Update()
    {
        // FPS
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " : Fps";

        // TIME
        hour = worldManager.GetDayDuration() / 24;
        minute = hour / 60;
        currentMinute = (int)((worldManager.GetTime() / minute) - (int)(worldManager.GetTime() / hour) * 60);
        timeText.text = "Time: " + ((int)(worldManager.GetTime() / hour)).ToString() + ":" + currentMinute.ToString();

        // VELOCITY
        Vector3 playerVelocity = player.GetVelocity();
        velocityText.text = playerVelocity.ToString();
        Vector3 speed = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        speedText.text = speed.magnitude.ToString();

        // CONTROLS PANEL
        if (Input.GetKeyDown(KeyCode.E))
        {
            b_controlsIsOpen = true;
            controlsPanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && b_controlsIsOpen)
        {
            controlsPanel.SetActive(false);
            b_controlsIsOpen = false;
        }
    }
}
