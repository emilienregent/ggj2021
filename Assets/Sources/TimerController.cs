using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public TMP_Text label = null;

    private int _maxGameDuration;
    private int _elapsedTime = 0;
    private int _timeLeft;

    private void Start()
    {
        _maxGameDuration = GameManager.instance.MaxGameDuration;
        _timeLeft = _maxGameDuration;
        InvokeRepeating("UpdateTimer", 1f, 1f);
    }

    private void UpdateTimer()
    {
        _elapsedTime++;
        _timeLeft = _maxGameDuration - _elapsedTime;

        label.text = _timeLeft.ToString();
    }
}