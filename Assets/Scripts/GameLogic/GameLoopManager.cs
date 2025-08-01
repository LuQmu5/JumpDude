using System.Collections;
using UnityEngine;

public class GameLoopManager
{
    private HintDisplay _hintDisplay;
    private PlayerController _player;
    private MainCameraController _mainCameraController;
    private Transform _playerSpawnPoint;
    private MonoBehaviour _coroutineRunner;
    private PlayerInput _input;

    private Coroutine _waitingForRestartRoutine;

    public GameLoopManager(HintDisplay hintDisplay,
        PlayerController player, MainCameraController mainCameraController, 
        Transform playerSpawnPoint, MonoBehaviour coroutineRunner, PlayerInput input)
    {
        _hintDisplay = hintDisplay;
        _player = player;
        _mainCameraController = mainCameraController;
        _playerSpawnPoint = playerSpawnPoint;
        _coroutineRunner = coroutineRunner;
        _input = input;
    }

    public void HandleLose()
    {
        if (_waitingForRestartRoutine != null)
            return;

        _hintDisplay.Show("You Lose\nPress 'R' to restart level");
        _mainCameraController.Deactivate();
        _player.Deactivate();

        _waitingForRestartRoutine = _coroutineRunner.StartCoroutine(WaitingForRestart());
    }

    public void RestartLevel()
    {
        _player.Activate();
        _player.transform.position = _playerSpawnPoint.position;
        _mainCameraController.Activate();
    }

    private IEnumerator WaitingForRestart()
    {
        yield return new WaitUntil(() => _input.GameLoop.Restart.triggered);

        RestartLevel();
        _hintDisplay.Hide();
        _waitingForRestartRoutine = null;
    }
}