using UnityEngine;

public class HookController : MonoBehaviour
{
    public Transform hookOrigin;
    public float maxDistance = 10f;
    public LayerMask hookableLayer;
    public float pullForce = 25f;

    private PlayerController player;

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    public bool TryFireHook(out Vector2 target)
    {
        target = Vector2.zero;

        #if UNITY_EDITOR
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - hookOrigin.position).normalized;
        #else
        Vector2 dir = player.input.MoveDirection.normalized;
        #endif

        RaycastHit2D hit = Physics2D.Raycast(hookOrigin.position, dir, maxDistance, hookableLayer);

        if (hit.collider != null)
        {
            target = hit.point;

            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                // Маленькое притягивание
                Vector2 pullDir = (hit.point - (Vector2)player.transform.position).normalized;
                player.rb.velocity = pullDir * (pullForce * 0.5f);
            }
            else
            {
                // Притягиваем к стене
                player.rb.velocity = (hit.point - (Vector2)player.transform.position).normalized * pullForce;
            }

            return true;
        }

        return false;
    }
}
