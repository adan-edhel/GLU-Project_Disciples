using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationHandler : MonoBehaviourPunCallbacks, ICharacterMovement, IPunObservable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color[] _playercolors;
    [SerializeField] private Color _localColor;

    private void Awake()
    {
        if (photonView.IsMine && PhotonNetwork.InRoom)
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                Photon.Realtime.Player Player = PhotonNetwork.CurrentRoom.Players[i+1];
                if (Player != null && Player.IsLocal)
                {
                    _renderer.color = _playercolors[i];
                }
            }
        }
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
