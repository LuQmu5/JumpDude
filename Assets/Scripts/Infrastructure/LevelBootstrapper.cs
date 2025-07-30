using UnityEngine;

public class LevelBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private MainCameraController _camera;

    private void Awake()
    {
        PlayerController playerInstance = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        playerInstance.Init();
        _camera.Init(playerInstance.transform);
    }
}