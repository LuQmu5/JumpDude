using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraController : MonoBehaviour
{
    private Transform _target;
    private Camera _camera;
    private Vector2 _cellSize;
    private Vector2Int _currentCell;

    public void Init(Transform target)
    {
        _target = target;

        if (_target == null)
        {
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
        float height = _camera.orthographicSize * 1.5f;
        float width = height * _camera.aspect;
        _cellSize = new Vector2(width, height);
    }

    private void UpdateCameraPosition(bool force = false)
    {
        if (_target == null)
            return;

        Vector2 targetPosition = _target.position;
        Vector2 offsetFromCenter = new Vector2(_cellSize.x / 2f, _cellSize.y / 2f);

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
