using UnityEngine;
using DG.Tweening;

public class MovableEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 moveDirection = Vector2.right;
    [SerializeField] private float moveDistance = 3f;

    private Vector3 _startPosition;
    private Tween _moveTween;

    private GameLoopManager _gameLoopManager;

    public void Init(GameLoopManager gameLoopManager)
    {
        _gameLoopManager = gameLoopManager;
    }

    private void Start()
    {
        _startPosition = transform.position;

        Vector3 targetOffset = (Vector3)moveDirection.normalized * moveDistance;
        Vector3 targetPosition = _startPosition + targetOffset;

        _moveTween = transform.DOMove(targetPosition, moveDistance / moveSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _moveTween?.Kill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            _gameLoopManager.HandleLose();
        }
    }
}
