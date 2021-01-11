using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class PlayerHandler : MonoBehaviourPunCallbacks, IOnPlayerDeath
{
    // Interfaces
    private ICharacterMovement[] iMovement;
    private ICharacterElement iAttack;
    private ICharacterAim[] iAim;
    private ITogglePause iPause;
    private IChat iChat;

    private GameObject _character;
    private PlayerInput _input;
    private GameObject _GUI;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _input = GetComponent<PlayerInput>();
        _GUI = GetComponentInChildren<HUD>().gameObject;
        gameObject.name = $"Observer ({photonView.Owner.NickName})";

        if (photonView.IsMine)
        {
            _input.enabled = true;
            _GUI.SetActive(false);
            iChat = GetComponent<IChat>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(_input);
            Destroy(_GUI);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (!SceneController.Instance.inMenu)
        {
            CreateCharacter();
        }
    }

    private void CreateCharacter()
    {
        _character = PhotonNetwork.Instantiate("Character/Character", Vector3.zero, Quaternion.identity);
        _character.gameObject.name = $"Character ({photonView.Owner.NickName})";

        iPause = GetComponentInChildren<ITogglePause>(includeInactive: true);
        iMovement = _character.GetComponents<ICharacterMovement>();
        iAttack = _character.GetComponent<ICharacterElement>();
        iAim = _character.GetComponents<ICharacterAim>();

        _character.GetComponent<CharacterBase>().OnPlayerDeath = this;

        for (int i = 0; i < iAim.Length; i++)
        {
            iAim[i]?.AssignInput(_input);
        }

        _GUI.SetActive(true);

        _input?.SwitchCurrentActionMap("Gameplay");
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
            if (SceneController.Instance.inMenu) return;

            if (_input?.currentActionMap.name == "Gameplay")
            {
                _input?.SwitchCurrentActionMap("UI");
                iPause?.TogglePause(true);
            }
            else
            {
                _input?.SwitchCurrentActionMap("Gameplay");
                iPause?.TogglePause(false);
            }
        }
    }

    public void OnChat(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    public void OnPlayerDeath()
    {
        _input?.SwitchCurrentActionMap("UI");
    }
}