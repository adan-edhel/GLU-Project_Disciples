using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandeler : MonoBehaviour, ICharacterMovement
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;

    public void AimInputValue(Vector2 input)
    {
        throw new System.NotImplementedException();
    }

    public void CutJump()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void MovementInputValue(Vector2 moveInput)
    {
        if (moveInput.x < 0)
        {
            _renderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            _renderer.flipX = false;
        }
    }

}
