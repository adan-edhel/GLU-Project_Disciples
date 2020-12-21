using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class PlayerHandler : MonoBehaviourPunCallbacks
{
    // Interfaces
    private ICharacterMovement[] iMovement;
    private ICharacterElement iAttack;
    private ICharacterAim[] iAim;
    private ITogglePause iPause;

    private GameObject character;
    private PlayerInput _input;
    private GameObject _GUI;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _input = GetComponent<PlayerInput>();
        _GUI = GetComponentInChildren<HUD>().gameObject;

        if (photonView.IsMine)
        {
            _input.enabled = true;
            _GUI.SetActive(true);
            gameObject.name = $"{PhotonNetwork.NickName} Observer";
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
        // Adjust Input Action Maps
        if (SceneController.Instance.inMenu)
        {
            _input.SwitchCurrentActionMap("UI");
        }
        else
        {
            CreateCharacter();
            _input.SwitchCurrentActionMap("Gameplay");
        }
    }

    private void CreateCharacter()
    {
        character = PhotonNetwork.Instantiate("Character/Character", Vector3.zero, Quaternion.identity);

        if (photonView.IsMine)
        {
            character.gameObject.name = photonView.Owner.NickName;

            iPause = character.GetComponentInChildren<ITogglePause>();
            iMovement = character.GetComponents<ICharacterMovement>();
            iAttack = character.GetComponent<ICharacterElement>();
            iAim = character.GetComponents<ICharacterAim>();

            for (int i = 0; i < iAim.Length; i++)
            {
                iAim[i].AssignInput(_input);
            }
        }
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

            if (_input.currentActionMap.name == "Gameplay")
            {
                _input.SwitchCurrentActionMap("UI");
                iPause.TogglePause(true);
            }
            else
            {
                _input.SwitchCurrentActionMap("Gameplay");
                iPause.TogglePause(false);
            }
        }
    }
}