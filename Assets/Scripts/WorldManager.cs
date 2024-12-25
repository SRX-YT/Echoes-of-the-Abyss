using UnityEngine;

class WorldManager : MonoBehaviour
{
    // Day Night Cycle
    [SerializeField] private Light directionalLight;
    [SerializeField] private float dayDuration = 86400; // Seconds in day (1 hour = 86400)
    private float time;

    private void Update()
    {
        DayNightCycle();
    }

    // TODO: (DAYNIGHTCYCLE) fix intensity light
    private void DayNightCycle()
    {
        // ����������� ����� � ����������� �� ������� �����
        time += Time.deltaTime;

        // ����������� �����, ����� ��� ���������� � �������� 0-1
        float normalizedTime = time / dayDuration;

        // ���� ����� ��������� 1, ���������� ���
        if (normalizedTime >= 1f)
        {
            normalizedTime = 0f;
            time = 0f;
        }

        // �������� ���� ��������� � ����������� �� ������� �����
        // �������� ����� �� 4 ���� ������ (0.33 � ��������������� �������)
        float sunAngle = (normalizedTime - 0.13f) % 1f; // ����� �� 4 ����
        directionalLight.transform.rotation = Quaternion.Euler((sunAngle * 360f) - 90, 170, 0);

        // �������� ������������� ����� � ����������� �� ������� �����
        if (sunAngle < 0.25f) // ���� (8:00 - 12:00)
        {
            directionalLight.intensity = Mathf.Lerp(0, 1, sunAngle / 0.25f); // ����������� ������������� �� 0 �� 1
        }
        else if (sunAngle < 0.75f) // ���� (12:00 - 20:00)
        {
            directionalLight.intensity = 1; // ������������ �������������
        }
        else if (sunAngle < 0.92f) // ����� (20:00 - 22:00)
        {
            directionalLight.intensity = Mathf.Lerp(1, 0.5f, (sunAngle - 0.75f) / 0.17f); // ��������� ������������� �� 1 �� 0.5
        }
        else // ���� (22:00 - 8:00)
        {
            directionalLight.intensity = Mathf.Lerp(0.5f, 0, (sunAngle - 0.92f) / 0.08f); // ��������� ������������� �� 0.5 �� 0
        }
    }

    public float GetTime()
    {
        return time;
    }

    public float GetDayDuration()
    {
        return dayDuration;
    }
}