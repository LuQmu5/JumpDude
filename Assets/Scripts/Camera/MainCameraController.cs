using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector2 _cellSize;
    private Vector2Int _currentCell;
    private Camera _camera;

    private void Start()
    {
        if (_target == null)
        {
            Debug.LogError("Camera target is not set!");
            enabled = false;
            return;
        }

        _camera = GetComponent<Camera>();
        CalculateCellSize();
        UpdateCameraPosition(force: true);
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void CalculateCellSize()
    {
        float height = _camera.orthographicSize * 2f;
        float width = height * _camera.aspect;
        _cellSize = new Vector2(width, height);
    }

    private void UpdateCameraPosition(bool force = false)
    {
        Vector2 targetPosition = _target.position;

        // Смещение для центрирования персонажа
        Vector2 offsetFromCenter = new Vector2(_cellSize.x / 2f, _cellSize.y / 2f);

        // Центрированные ячейки (чтобы камера "обрамляла" персонажа)
        Vector2Int targetCell = new Vector2Int(
            Mathf.FloorToInt((targetPosition.x + offsetFromCenter.x) / _cellSize.x),
            Mathf.FloorToInt((targetPosition.y + offsetFromCenter.y) / _cellSize.y)
        );

        if (force || targetCell != _currentCell)
        {
            _currentCell = targetCell;

            Vector2 cellCenter = new Vector2(
                _currentCell.x * _cellSize.x,
                _currentCell.y * _cellSize.y
            );

            transform.position = new Vector3(cellCenter.x, cellCenter.y, transform.position.z);
        }
    }
}
