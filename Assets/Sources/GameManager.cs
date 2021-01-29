using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance { private set; get; }

    private void Awake()
    {
        // First destroy any existing instance of it
        if (instance != null)
        {
            Destroy(instance);
        }

        // Then reassign a proper one
        instance = this;
    }
    #endregion

    [Header("Configuration")]
    public float ItemSpawDelay = 1f;
    public float ItemSpawnInterval = 2f;
    public float CustomerSpawDelay = 1f;
    public float CustomerSpawnInterval = 2f;

    [Header("Game Values")]
    public int currentScore = 0;

    public Action<int> ScoreUpdated = null;

    public void UpdateScore(int score)
    {
        currentScore += score;

        if (ScoreUpdated != null)
        {
            ScoreUpdated.Invoke(currentScore);
        }
    }
}
