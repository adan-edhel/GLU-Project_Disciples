using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class AnimationHandler : MonoBehaviourPunCallbacks, ICharacterMovement, IPunObservable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CharecterColors _playercolors;
    [SerializeField] private Color _localColor;
    private int _movementState;

    private void Awake()
    {
        if (photonView.IsMine && PhotonNetwork.InRoom)
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                Photon.Realtime.Player Player = PhotonNetwork.CurrentRoom.Players[i+1];
                if (Player != null && Player.IsLocal)
                {
                    _renderer.color = _playercolors.getColors[i];
                }
            }
        }
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetRenderColor(Color Color)
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
        if (PhotonNetwork.InRoom && photonView.IsMine)
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

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.y > 0.20f )
        {
            _movementState = 2;
            _animator.SetInteger("Movementstate", _movementState);
        }
        else if (_rigidbody.velocity.y < -0.20f)
        {
            _movementState = 3;
            _animator.SetInteger("Movementstate", _movementState);
        }
        else if ( Vector2.Distance(Vector2.zero, _rigidbody.velocity) <= 1 && _movementState != 0)
        {
            _movementState = 0;
            _animator.SetInteger("Movementstate", _movementState);
        }
        else if (_rigidbody.velocity.x != 0 && _movementState != 1)
        {
            _movementState = 1;
            _animator.SetInteger("Movementstate", _movementState);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.name);
            stream.SendNext(_renderer.color.r);
            stream.SendNext(_renderer.color.g);
            stream.SendNext(_renderer.color.b);
            stream.SendNext(_renderer.color.a);
            stream.SendNext(_renderer.flipX);
        }
        else if (stream.IsReading)
        {
            gameObject.name = (string)stream.ReceiveNext();
            _localColor.r = (float)stream.ReceiveNext();
            _localColor.g = (float)stream.ReceiveNext();
            _localColor.b = (float)stream.ReceiveNext();
            _localColor.a = (float)stream.ReceiveNext();
            this._renderer.color = _localColor;
            this._renderer.flipX = (bool)stream.ReceiveNext();
        }
    }
}
