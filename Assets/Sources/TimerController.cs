using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public TMP_Text label = null;

    private void Start()
    {
        GameManager.instance.TimerUpdated += OnTimerUpdated;
    }

    private void OnTimerUpdated(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft - (minutes * 60f));

        label.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TimerUpdated -= OnTimerUpdated;
        }
    }
}