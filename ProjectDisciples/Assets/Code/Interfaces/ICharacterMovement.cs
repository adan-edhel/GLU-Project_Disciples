using UnityEngine;

public interface ICharacterMovement
{
    void HandleAim(Vector2 input);
    void HandleMovement(Vector2 moveInput);
    void Jump();
}