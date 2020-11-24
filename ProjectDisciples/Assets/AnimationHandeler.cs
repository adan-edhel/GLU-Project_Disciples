using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandeler : MonoBehaviour, ICharacterMovement
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;

    public void AimInputValue(Vector2 input)
    {
       
    }

    public void CutJump()
    {
       
    }

    public void Jump()
    {
      
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
