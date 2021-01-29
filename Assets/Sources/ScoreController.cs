using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TMP_Text label = null;

    private int _currentScore = 0;
    private int _tweenId = -1;

    private void Start()
    {
        GameManager.instance.ScoreUpdated += OnScoreUpdated;
    }

    public void OnScoreUpdated(int score)
    {
        if (LeanTween.isTweening(_tweenId) == true)
        {
            LeanTween.cancel(gameObject, _tweenId);
        }

        _tweenId = LeanTween.value(_currentScore, score, 1f).setOnUpdate(OnAnimationUpdated).id;
    }

    private void OnAnimationUpdated(float progress)
    {
        _currentScore = Mathf.RoundToInt(progress);

        label.text = _currentScore.ToString();
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ScoreUpdated -= OnScoreUpdated;
        }
    }
}