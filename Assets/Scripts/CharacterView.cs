using UnityEngine;

public class CharacterView
{
    private readonly SpriteRenderer _spriteRenderer;

    public CharacterView(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public void FlipByDirection(Vector2 direction)
    {
        if (direction.x == 0)
            return;

        Vector3 scale = _spriteRenderer.transform.localScale;
        scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
        _spriteRenderer.transform.localScale = scale;
    }
}
