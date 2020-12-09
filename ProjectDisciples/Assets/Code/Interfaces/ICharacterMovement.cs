using UnityEngine;

public interface ICharacterMovement
{
    void MovementInputValue(Vector2 moveInput);
    void Jump();
    void CutJump();
}