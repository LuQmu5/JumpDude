using UnityEngine;

public class JumpKingCamera : MonoBehaviour
{
    [SerializeField] private Transform _target; 
    [SerializeField] private Vector2 _cellSize = new Vector2(16, 9); 
    [SerializeField] private Vector2 _offset = Vector2.zero;  

    private Vector2Int _currentCell;

    private void Start()
    {
        if (_target == null)
            Debug.LogError("Camera target is not set!");

        UpdateCameraPosition(force: true);
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition(bool force = false)
    {
        Vector2 targetPosition = _target.position;

        Vector2Int targetCell = new Vector2Int(
            Mathf.FloorToInt(targetPosition.x / _cellSize.x),
            Mathf.FloorToInt(targetPosition.y / _cellSize.y)
        );

        if (force || targetCell != _currentCell)
        {
            _currentCell = targetCell;

            Vector2 cellCenter = new Vector2(
                _currentCell.x * _cellSize.x + _cellSize.x / 2f,
                _currentCell.y * _cellSize.y + _cellSize.y / 2f
            );

            Vector3 newCameraPosition = new Vector3(cellCenter.x + _offset.x, cellCenter.y + _offset.y, transform.position.z);
            transform.position = newCameraPosition;
        }
    }
}
