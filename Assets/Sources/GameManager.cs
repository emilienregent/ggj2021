using System;
using UnityEngine;

public enum GameState { Tutorial, Preparation, Wave, Gameover};

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

    [Header("Configuration - Game")]
    [Tooltip("Duration of the game in seconds. At 0 it's Game Over")]
    public int MaxGameDuration = 120;
    public int CurrentWave = 0;
    public int MaxWave = 10;
    public float PreparationPhaseDuration = 10;
    public float WavePhaseDuration = 30;
    public int[] RequiredClientPerWave;
    public GameState _currentGameState = GameState.Tutorial;

    float _currentPreparationPhaseDuration;
    float _currentWavePhaseDuration;

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

    private void Start() {
        _currentPreparationPhaseDuration = PreparationPhaseDuration;
        _currentWavePhaseDuration = WavePhaseDuration;
    }

    private void Update() {
        switch(GameManager.instance._currentGameState)
        {
            case GameState.Preparation:
                _currentPreparationPhaseDuration -= Time.deltaTime;
                if(_currentPreparationPhaseDuration <= 0)
                {
                    _currentGameState = GameState.Wave;
                    _currentPreparationPhaseDuration = PreparationPhaseDuration;
                    UpdateSpawnInterval();
                }
                break;
            case GameState.Wave:
                _currentWavePhaseDuration -= Time.deltaTime;
                if(_currentWavePhaseDuration <= 0)
                {
                    //if(_totalCustomer < RequiredClientPerWave[CurrentWave])
                    //{
                    //    _currentGameState = GameState.Gameover;
                    //} else
                    //{
                        _currentGameState = GameState.Preparation;
                        UpdateSpawnInterval();
                    //}

                _currentWavePhaseDuration = WavePhaseDuration;
                }
                break;
            case GameState.Gameover:
                break;
            case GameState.Tutorial:
                break;
        }
    }

    public void UpdateScore(int score)
    {
        currentScore += score;

        _totalCustomer++;
        UpdateSpawnInterval();

        if (ScoreUpdated != null)
        {
            ScoreUpdated.Invoke(currentScore);
        }

        CoinSpawner.instance.AddCoinsToSpawn(score);

        if(CurrentWave == 0 && _totalCustomer == 1)
        {
            _currentGameState = GameState.Preparation;
        }
    }

    void UpdateSpawnInterval() {
        switch(GameManager.instance._currentGameState)
        {
            case GameState.Preparation:
                ItemSpawnInterval = 3f;
                break;
            
            case GameState.Wave:
                ItemSpawnInterval -= Mathf.Min(MaxCustomerSpawnInterval, Mathf.Max(MinCustomerSpawnInterval, CustomerSpawnOverTime.Evaluate(_totalCustomer)));
                CustomerSpawnInterval -= Mathf.Min(MaxCustomerSpawnInterval, Mathf.Max(MinCustomerSpawnInterval, CustomerSpawnOverTime.Evaluate(_totalCustomer)));
                break;
        }
    }
}
