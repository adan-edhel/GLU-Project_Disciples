using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class InputHandler : MonoBehaviourPunCallbacks
{
    // Interfaces
    ICharacterMovement[] iMovement;
    ICharacterElement iAttack;
    ICharacterAim[] iAim;

    private void Start()
    {
        if (PhotonNetwork.InRoom && photonView.IsMine)
        {
            GetComponent<PlayerInput>().enabled = true;

            iMovement = GetComponents<ICharacterMovement>();
            iAttack = GetComponent<ICharacterElement>();
            iAim = GetComponents<ICharacterAim>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        CameraManager.Instance.virtualCamera.Follow = gameObject.transform;
        transform.position = Vector3.zero;
    }

    /// <summary>
    /// Gets aim input and sends it through an interface
    /// </summary>
    /// <param name="context"></param>
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.action.ReadValue<Vector2>() != Vector2.zero && iAim != null)
        {
            for (int i = 0; i < iAim.Length; i++)
            {
                iAim[i]?.AimInputValue(context.action.ReadValue<Vector2>());
            }
        }
    }

    /// <summary>
    /// Gets movement input and sends it through an interface
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && iMovement != null)
        {
            for (int i = 0; i < iMovement.Length; i++)
            {
                iMovement[i]?.MovementInputValue(context.action.ReadValue<Vector2>());
            }
        }
    }

    /// <summary>
    /// Gets jump input and sets its bool
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (iMovement == null) return;

        for (int i = 0; i < iMovement.Length; i++)
        {
            if (context.performed)
            {
                iMovement[i]?.Jump();
            }

            if (context.canceled)
            {
                iMovement[i]?.CutJump();
            }
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
    /// Gets switch element input and calls its function
    /// </summary>
    /// <param name="context"></param>
    public void OnPrevElement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            iAttack?.SwitchPreviousElement();
        }
    }

    /// <summary>
    /// Gets switch element input and calls its function
    /// </summary>
    /// <param name="context"></param>
    public void OnNextElement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            iAttack?.SwitchNextElement();
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