using System;
using TMPro;
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
    public Color PreparationPhaseColor = Color.white;
    public Color WavePhaseColor = Color.white;
    public int[] RequiredClientPerWave;

    public SlidingText PreparationPhaseAnnounce;
    public SlidingText WavePhaseAnnounce;
    public TextMeshPro WaveGoal;

    [Header("Configuration - Items")]
    public float ItemSpawDelay = 1f;
    public float ItemSpawnInterval = 2f;
    public AnimationCurve ItemSpawnOverTime;

    [Header("Configuration - Customers")]
    public int CustomerMaxNumber = 8;
    public float CustomerSpawDelay = 1f;
    public float CustomerSpawnInterval = 2f;
    public AnimationCurve CustomerSpawnOverTime;

    float MaxItemSpawnInterval = 3f;
    float MinItemSpawnInterval = 0.5f;
    float MaxCustomerSpawnInterval = 7f;
    float MinCustomerSpawnInterval = 0.5f;
    int _totalCustomer = 0;

    [Header("Game Values")]
    public int currentScore = 0;

    public Action<int> ScoreUpdated = null;
    public Action<float> TimerUpdated = null;
    public Action<GameState> StateUpdated = null;

    public float GameTime = 0f;

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
                GameTime += Time.deltaTime;

                if (CurrentTimer <= 0)
                {
                     StartWavePhase();
                }

                break;

            case GameState.Wave:
                CurrentTimer -= Time.deltaTime;
                GameTime += Time.deltaTime;

                if (CurrentTimer <= 0)
                {
                    EndOfWave();
                    
                }

                break;

            case GameState.Gameover:
                break;

            case GameState.Tutorial:
                GameTime += Time.deltaTime;
                break;
        }
    }

    private void EndOfWave()
    {
        if (_totalCustomer < RequiredClientPerWave[CurrentWave])
        {
            PopupController.instance.GameOverPopup.Display();
            _currentGameState = GameState.Gameover;
        } else
        {
            StartPreparationPhase();
        }
    }

    private void StartPreparationPhase()
    {
        CurrentTimer = PreparationPhaseDuration;
        GameState previousState = CurrentGameState;
        CurrentGameState = GameState.Preparation;
        PreparationPhaseAnnounce.enabled = true;
        UpdateSpawnInterval();
        if(previousState != GameState.Tutorial)
        {
            CurrentWave++;
            ItemSpawner.instance.RefreshItemList();
        }
        _totalCustomer = 0;
        WaveGoal.text = _totalCustomer + " / " + RequiredClientPerWave[CurrentWave];
    }

    private void StartWavePhase()
    {
        CurrentTimer = WavePhaseDuration;
        CurrentGameState = GameState.Wave;

        WavePhaseAnnounce.PanelText.text = "Wave " + (CurrentWave + 1).ToString() + " / "+ MaxWave.ToString();
        WavePhaseAnnounce.enabled = true;

        UpdateSpawnInterval();
    }

    public void UpdateScore(int score)
    {
        currentScore += score;

        _totalCustomer++;
        WaveGoal.text = _totalCustomer + " / " + RequiredClientPerWave[CurrentWave];
        UpdateSpawnInterval();

        if (ScoreUpdated != null)
        {
            ScoreUpdated.Invoke(currentScore);
        }

        CoinSpawner.instance.AddCoinsToSpawn(score);

        if(CurrentWave == 0 && _totalCustomer == 1 && CurrentGameState == GameState.Tutorial)
        {
            StartPreparationPhase();
        }
    }

    private void UpdateSpawnInterval() {
        switch(CurrentGameState)
        {
            case GameState.Preparation:
                ItemSpawnInterval = Mathf.Min(MaxItemSpawnInterval, Mathf.Max(MinItemSpawnInterval, (MaxItemSpawnInterval - ItemSpawnOverTime.Evaluate(GameTime))));
                break;
            
            case GameState.Wave:
                ItemSpawnInterval = 3f;
                CustomerSpawnInterval -= Mathf.Min(MaxCustomerSpawnInterval, Mathf.Max(MinCustomerSpawnInterval, CustomerSpawnOverTime.Evaluate(_totalCustomer)));
                break;
        }
    }
}
