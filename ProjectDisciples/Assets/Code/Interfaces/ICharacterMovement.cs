using UnityEngine;

public interface ICharacterMovement
{
    void AimInputValue(Vector2 input);
    void MovementInputValue(Vector2 moveInput);
    void Jump();
    void CutJump();
}