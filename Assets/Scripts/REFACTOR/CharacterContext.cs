using UnityEngine;

public class CharacterContext
{
    public Rigidbody2D Rigidbody { get; set; }
    public Transform Transform { get; set; }
    public Vector2 InputDirection { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsFalling => Rigidbody.velocity.y < 0;
    public bool IsRising => Rigidbody.velocity.y > 0;
    public bool IsGliding { get; set; }
    public bool IsDashing { get; set; }
    public bool IsSliding { get; set; }
    public bool CanMove => !IsDashing;

    public CharacterContext(Rigidbody2D rb, Transform transform)
    {
        Rigidbody = rb;
        Transform = transform;
    }
}
