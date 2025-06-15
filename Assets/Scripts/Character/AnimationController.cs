using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerController player;

    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(player.rb.linearVelocity.x));
        anim.SetBool("IsGrounded", player.IsGrounded);
        anim.SetFloat("VerticalSpeed", player.rb.linearVelocity.y);
    }

    public void PlayHook() => anim.SetTrigger("Hook");
    public void PlayJump() => anim.SetTrigger("Jump");
    public void PlayDash() => anim.SetTrigger("Dash");
    public void PlayGlide(bool value) => anim.SetBool("IsGliding", value);
    public void PlayWallCling() => anim.SetTrigger("WallCling");

}

