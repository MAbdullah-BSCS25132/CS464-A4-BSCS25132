using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image lag;
    [SerializeField] private float lagDelay = 0.4f;
    [SerializeField] private float lagSpeed = 0.8f;   // har second kitna ghatta hai

    private float target = 1f;
    private float delayLeft;

    public void Set(int current, int max)
    {
        float t = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;

        // Nuksan hua: lag bar pehle 0.4 s ruki rahe
        if (t < target) delayLeft = lagDelay;

        target = t;
        fill.fillAmount = t;

        // Bhar gaya ya naya game: lag bar foran upar
        if (t > lag.fillAmount) lag.fillAmount = t;
    }

    private void Update()
    {
        if (lag.fillAmount <= target) return;

        if (delayLeft > 0f)
        {
            delayLeft -= Time.deltaTime;
            return;
        }

        lag.fillAmount = Mathf.MoveTowards(lag.fillAmount, target, lagSpeed * Time.deltaTime);
    }
}