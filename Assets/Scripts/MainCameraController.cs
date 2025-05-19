using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [Header("Цель слежения")]
    public Transform target;

    [Header("Настройки сглаживания")]
    public float smoothTime = 0.3f;

    [Header("Ограничения камеры")]
    public float minY = 0f;   // Минимальное Y (например, пол)
    public float maxY = Mathf.Infinity; // Максимальное Y (можно ограничить, если нужно)

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Целевая позиция — позиция игрока по X и Y, камера фиксируется по Z
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Ограничиваем позицию камеры по Y
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        // Плавно двигаем камеру к целевой позиции с использованием SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
