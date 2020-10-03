using UnityEngine;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        _gameStateHandler = new GameStateHandler();

        timerManager.Initialize(_gameStateHandler);
        grid.Initialize(timerManager, _gameStateHandler);

        _gameStateHandler.GameStateEvent?.Invoke(GameStateEnum.Play);
    }

    [SerializeField] private TimerManager timerManager = default;

    [SerializeField] private Grid grid = default;

    private GameStateHandler _gameStateHandler;
}