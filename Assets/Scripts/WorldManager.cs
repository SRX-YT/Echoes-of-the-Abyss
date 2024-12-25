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
        // Увеличиваем время в зависимости от времени кадра
        time += Time.deltaTime;

        // Нормализуем время, чтобы оно оставалось в пределах 0-1
        float normalizedTime = time / dayDuration;

        // Если время превышает 1, сбрасываем его
        if (normalizedTime >= 1f)
        {
            normalizedTime = 0f;
            time = 0f;
        }

        // Изменяем угол освещения в зависимости от времени суток
        // Сдвигаем время на 4 часа вперед (0.33 в нормализованном времени)
        float sunAngle = (normalizedTime - 0.13f) % 1f; // Сдвиг на 4 часа
        directionalLight.transform.rotation = Quaternion.Euler((sunAngle * 360f) - 90, 170, 0);

        // Изменяем интенсивность света в зависимости от времени суток
        if (sunAngle < 0.25f) // Утро (8:00 - 12:00)
        {
            directionalLight.intensity = Mathf.Lerp(0, 1, sunAngle / 0.25f); // Увеличиваем интенсивность от 0 до 1
        }
        else if (sunAngle < 0.75f) // День (12:00 - 20:00)
        {
            directionalLight.intensity = 1; // Максимальная интенсивность
        }
        else if (sunAngle < 0.92f) // Вечер (20:00 - 22:00)
        {
            directionalLight.intensity = Mathf.Lerp(1, 0.5f, (sunAngle - 0.75f) / 0.17f); // Уменьшаем интенсивность от 1 до 0.5
        }
        else // Ночь (22:00 - 8:00)
        {
            directionalLight.intensity = Mathf.Lerp(0.5f, 0, (sunAngle - 0.92f) / 0.08f); // Уменьшаем интенсивность от 0.5 до 0
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