using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetVelocityX(int value)
    {
        _animator.SetInteger("VelocityX", value);
    }

    public void SetVelocityY(int value)
    {
        _animator.SetInteger("VelocityY", value);
    }

    public void SetOnGroundState(bool state)
    {
        _animator.SetBool("OnGround", state);
    }

    public void SetGlidingState(bool state)
    {
        _animator.SetBool("IsGliding", state);
    }

    public void SetJumpTrigger()
    {
        _animator.SetTrigger("Jump");
    }

    public void SetDashTrigger()
    {
        _animator.SetTrigger("Dash");
    }

    public void SetFallState(bool state)
    {
        _animator.SetBool("IsFalling", state);
    }
}
