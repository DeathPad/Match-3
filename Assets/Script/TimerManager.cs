using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float currentTimer { get; private set; }
    
    public void Initialize(GameStateHandler gameStateHandler)
    {
        _gameStateHandler = gameStateHandler;
        _gameStateHandler.GameStateEvent -= GameStateChangedEvent;
        _gameStateHandler.GameStateEvent += GameStateChangedEvent;
    }

    //Update score dan ui
    public void ResetTimer()
    {
        _currentTimer = DEFAULT_TIMER;
    }

    private void Update()
    {
        if (_gameStateEnum == GameStateEnum.Play)
        {
            _currentTimer -= Time.deltaTime;
            if(_currentTimer <= 0)
            {
                _gameStateHandler.GameStateEvent?.Invoke(GameStateEnum.Lose);
            }

            remainingTimeText.text = _currentTimer.ToString();
        }
    }

    private void GameStateChangedEvent(GameStateEnum gameStateEnum)
    {
        _gameStateEnum = gameStateEnum;

        if(_gameStateEnum == GameStateEnum.Play)
        {
            _currentTimer = DEFAULT_TIMER;
        }
    }

    private void OnDestroy()
    {
        _gameStateHandler.GameStateEvent -= GameStateChangedEvent;
    }

    [SerializeField] private Text remainingTimeText = default;

    private GameStateHandler _gameStateHandler;
    private GameStateEnum _gameStateEnum = GameStateEnum.Idle;

    private const float DEFAULT_TIMER = 15f;
    private float _currentTimer = DEFAULT_TIMER;
}