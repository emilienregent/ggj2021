using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TMP_Text label = null;

    private int _currentScore = 0;
    private int _tweenId = -1;

    public void SetScore(int score)
    {
        if (LeanTween.isTweening(_tweenId) == true)
        {
            LeanTween.cancel(gameObject, _tweenId);
        }

        _tweenId = LeanTween.value(_currentScore, score, 1f).setOnUpdate(OnScoreAnimationUpdated).id;
    }

    private void Start()
    {
        StartCoroutine(FakeUpdateScore());
    }

    private void OnScoreAnimationUpdated(float progress)
    {
        _currentScore = Mathf.RoundToInt(progress);

        label.text = _currentScore.ToString();
    }

    private IEnumerator FakeUpdateScore()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        SetScore(_currentScore + Random.Range(5, 10));

        StartCoroutine(FakeUpdateScore());
    }
}