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

    [Header("Configuration - Items")]
    public float ItemSpawDelay = 1f;
    public float ItemSpawnInterval = 2f;
    public AnimationCurve ItemSpawnOverTime;

    [Header("Configuration - Customers")]
    public int CustomerMaxNumber = 8;
    public float CustomerSpawDelay = 1f;
    public float CustomerSpawnInterval = 2f;
    public AnimationCurve CustomerSpawnOverTime;

    float MaxItemSpawnInterval = 5f;
    float MinItemSpawnInterval = 0.5f;
    float MaxCustomerSpawnInterval = 7f;
    float MinCustomerSpawnInterval = 0.5f;
    int _totalCustomer = 0;

    [Header("Game Values")]
    public int currentScore = 0;

    public Action<int> ScoreUpdated = null;

    public void UpdateScore(int score)
    {
        currentScore += score;

        _totalCustomer++;
        ItemSpawnInterval -= Mathf.Min(MaxItemSpawnInterval, Mathf.Max(MinItemSpawnInterval, ItemSpawnOverTime.Evaluate(_totalCustomer)));
        CustomerSpawnInterval -= Mathf.Min(MaxCustomerSpawnInterval, Mathf.Max(MinCustomerSpawnInterval, CustomerSpawnOverTime.Evaluate(_totalCustomer)));

        if (ScoreUpdated != null)
        {
            ScoreUpdated.Invoke(currentScore);
        }
    }
}
