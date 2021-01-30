using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public TMP_Text label = null;
    public Image background = null;

    private void Start()
    {
        GameManager.instance.TimerUpdated += OnTimerUpdated;
        GameManager.instance.StateUpdated += OnStateUpdated;

        OnStateUpdated(GameManager.instance.CurrentGameState);
    }

    private void OnTimerUpdated(float timeLeft)
    {
        if(timeLeft < 0)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft - (minutes * 60f));

        label.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    private void OnStateUpdated(GameState state)
    {
        switch(state)
        {
            case GameState.Tutorial:
                label.text = "Tutorial";
            break;
            case GameState.Preparation:
                background.color = GameManager.instance.PreparationPhaseColor;
            break;
            case GameState.Wave:
                background.color = GameManager.instance.WavePhaseColor;
            break;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TimerUpdated -= OnTimerUpdated;
            GameManager.instance.StateUpdated -= OnStateUpdated;
        }
    }
}