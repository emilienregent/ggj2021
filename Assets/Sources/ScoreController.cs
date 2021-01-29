using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TMP_Text label = null;

    private int currentScore = 0;

    public void SetScore(int score)
    {
        currentScore = score;

        label.text = currentScore.ToString();
    }

    private void Start()
    {
        StartCoroutine(FakeUpdateScore());
    }

    private IEnumerator FakeUpdateScore()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        SetScore(currentScore + Random.Range(5, 10));

        StartCoroutine(FakeUpdateScore());
    }
}