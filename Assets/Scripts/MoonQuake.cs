using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MoonQuake : MonoBehaviour
{
    public float totalTime = 300f; // 5 minut w sekundach
    public Text timerText;

    private float remainingTime;

    void Start()
    {
        remainingTime = totalTime;
    }

    void Update()
    {
        if(remainingTime % 60==0)
        {
            CameraShake.Instance.Shake(1f, 0.01f);
        }
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            remainingTime = 0;
            CameraShake.Instance.Shake(0.5f, 0.2f);
            UpdateTimerDisplay();
            // Mo¿esz tu dodaæ co ma siê staæ po zakoñczeniu czasu
            Debug.Log("Czas min¹³!");
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
