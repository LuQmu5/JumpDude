using UnityEngine;

public class LevelBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private MainCameraController _camera;
    [SerializeField] private HintDisplay _hintDisplay;
    [SerializeField] private DeadZoneTrigger _deadZoneTrigger;
    [SerializeField] private MovableEnemy[] _enemies;

    private void Awake()
    {
        PlayerInput playerInput = new PlayerInput();
        PlayerController playerInstance = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        playerInstance.Init(playerInput);

        TutorialManager tutorialManager = new TutorialManager(_hintDisplay, playerInput);
        GameLoopManager gameLoopManager = new GameLoopManager(_hintDisplay, playerInstance, _camera, _playerSpawnPoint, this, playerInput);

        _camera.Init(playerInstance.transform);
        _deadZoneTrigger.Init(gameLoopManager);

        foreach (var enemy in _enemies)
        {
            enemy.Init(gameLoopManager);
        }
    }
}