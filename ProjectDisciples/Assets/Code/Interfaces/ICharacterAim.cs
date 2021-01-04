using UnityEngine.InputSystem;
using UnityEngine;

public interface ICharacterAim
{
    void AimInputValue(Vector2 input);
    void AssignInput(PlayerInput input);
}
