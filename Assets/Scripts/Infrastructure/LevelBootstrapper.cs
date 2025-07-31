using UnityEngine;

public class LevelBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private MainCameraController _camera;
    [SerializeField] private HintDisplay _hintDisplay;

    private void Awake()
    {
        PlayerInput playerInput = new PlayerInput();
        PlayerController playerInstance = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        playerInstance.Init(playerInput);

        _camera.Init(playerInstance.transform);

        TutorialManager tutorialManager = new TutorialManager(_hintDisplay, playerInput);
    }
}