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

    [SerializeField] private GameState _currentGameState = GameState.Tutorial;
    [SerializeField] private float _currentTimer;

    [Space]

    [Header("Configuration - Game")]
    [Tooltip("Duration of the game in seconds. At 0 it's Game Over")]
    public int MaxGameDuration = 120;
    public int CurrentWave = 0;
    public int MaxWave = 10;
    public float PreparationPhaseDuration = 10;
    public float WavePhaseDuration = 30;
    public int[] RequiredClientPerWave;

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
    public Action<float> TimerUpdated = null;
    public Action<GameState> StateUpdated = null;

    public float CurrentTimer
    {
        get { return _currentTimer; }
        set
        {
            _currentTimer = value;

            if (TimerUpdated != null)
            {
                TimerUpdated.Invoke(value);
            }
        }
    }

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            _currentGameState = value;

            if (StateUpdated != null)
            {
                StateUpdated.Invoke(value);
            }
        }
    }

    private void Update()
    {
        switch(CurrentGameState)
        {
            case GameState.Preparation:
                CurrentTimer -= Time.deltaTime;

                if(CurrentTimer <= 0)
                {
                     StartWavePhase();
                }

                break;
            case GameState.Wave:

                CurrentTimer -= Time.deltaTime;

                if (CurrentTimer <= 0)
                {
                    StartPreparationPhase();
                }

                break;
            case GameState.Gameover:
                break;
            case GameState.Tutorial:
                break;
        }
    }

    private void StartPreparationPhase()
    {
        //if(_totalCustomer < RequiredClientPerWave[CurrentWave])
        //{
        //    _currentGameState = GameState.Gameover;
        //} else
        //{
        CurrentTimer = PreparationPhaseDuration;
        CurrentGameState = GameState.Preparation;

        UpdateSpawnInterval();
        //}
    }

    private void StartWavePhase()
    {
        CurrentTimer = WavePhaseDuration;
        CurrentGameState = GameState.Wave;

        UpdateSpawnInterval();
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
            CurrentGameState = GameState.Preparation;
        }
    }

    private void UpdateSpawnInterval() {
        switch(CurrentGameState)
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
