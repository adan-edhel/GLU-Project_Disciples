using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // Interfaces
    ICharacterMovement[] iMovement;
    ICharacterAttack iAttack;

    private void Start()
    {
        iMovement = GetComponents<ICharacterMovement>();
        iAttack = GetComponent<ICharacterAttack>();
    }

    /// <summary>
    /// Gets aim input and sends it through an interface
    /// </summary>
    /// <param name="context"></param>
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.action.ReadValue<Vector2>() != Vector2.zero)
        {
            iMovement[0]?.HandleAim(context.action.ReadValue<Vector2>());
        }
    }

    /// <summary>
    /// Gets movement input and sends it through an interface
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < iMovement.Length; i++)
            {
                iMovement[i]?.HandleMovement(context.action.ReadValue<Vector2>());
            }
        }
    }

    /// <summary>
    /// Gets jump input and sets its bool
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            iMovement[0].Jump();
        }
    }

    /// <summary>
    /// Gets attack input and calls its function
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            iAttack?.Attack1();
        }
    }

    /// <summary>
    /// Gets attack input and calls its function
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            iAttack?.Attack2();
        }
    }

    /// <summary>
    /// Gets pause input and sets its bool
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
}