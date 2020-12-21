using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterAim : MonoBehaviourPunCallbacks, ICharacterAim
{
    [Header("Aim Variables")]
    public Vector2 aimDirection;
    public float aimAngle;
    public float crosshairDistance = 1f;
    public GameObject Crosshair;

    private PlayerInput _input;

    Vector2 i_aimInput;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Crosshair.GetComponent<SpriteRenderer>().enabled = false;

            // If not in menu, assign crosshair to camera
            CameraManager.Instance.AssignFollowTargets(gameObject, Crosshair);
        }
    }

    private void Update()
    {
        // if not paused

        HandleAim();
        SetCrosshairPosition();
    }

    /// <summary>
    /// Uses aim input to reposition the crosshairs
    /// </summary>
    public void HandleAim()
    {
        if (!PhotonNetwork.InRoom || !photonView.IsMine) return;

        if (_input.currentControlScheme == "PC")
        {
            var worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var facingDirection = worldMousePosition - transform.position;
            aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        }
        else
        {
            // Controller Aim Angle
            aimAngle = Mathf.Atan2(i_aimInput.y, i_aimInput.x);
            // Only the calculation here needs work
            // Vector 2 i_aimInput needs to be converted

            //https://forum.unity.com/threads/determining-rotation-and-converting-to-a-2d-direction-vector.416277/
            //https://answers.unity.com/questions/927323/how-to-get-smooth-analog-joystick-rotation-without.html
        }

        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
    }

    private void SetCrosshairPosition()
    {
        var x = transform.position.x + crosshairDistance * Mathf.Cos(aimAngle);
        var y = transform.position.y + crosshairDistance * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        Crosshair.transform.position = crossHairPosition;
    }

    /// <summary>
    /// Takes aim input into a local variable
    /// </summary>
    /// <param name="input"></param>
    public void AimInputValue(Vector2 input)
    {
        i_aimInput = input;
    }

    public void AssignInput(PlayerInput input)
    {
        _input = input;
    }
}
