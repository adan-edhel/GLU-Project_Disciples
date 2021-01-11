using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CharecterChatter : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool ChatterActive = false;
    [SerializeField] private ChatterPanel _textPanel;
    [SerializeField] private CharecterColors _charecterColors;
    [SerializeField] private int _intPlayerColor;

    [SerializeField] private Image[] _backgrounds;
    [SerializeField] private TMP_InputField _inputfield;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            _textPanel.SetAsStatic();
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                Photon.Realtime.Player Player = PhotonNetwork.CurrentRoom.Players[i + 1];
                if (Player != null && Player.IsLocal)
                {
                    _intPlayerColor = i;
                }
            }
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }

    }

    [PunRPC]
    public void SetMessage(string Message)
    {
        if (ChatterPanel.Instance!=null)
        {
            ChatterPanel.Instance.SetMessage(Message);
        }
    }

    public void RPCSendMessage(string Message)
    {
        if (Message != string.Empty)
        {
            Message = $"<color=#{ColorUtility.ToHtmlStringRGB(_charecterColors.getColors[_intPlayerColor])}> {photonView.Owner.NickName} </color>: {Message}\n";
            photonView.RPC("SetMessage", RpcTarget.All, Message);
            _inputfield.text = string.Empty;
        }
        Chatting(false);
    }

    public void Chatting(bool StartChatting)
    {
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _backgrounds[i].enabled = StartChatting;
        }

        _inputfield.gameObject.SetActive(StartChatting);

        if (StartChatting)
            _inputfield.Select();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            SetMessage($"{newPlayer.NickName} has joined\n");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (photonView.IsMine)
        {
            SetMessage($"{otherPlayer.NickName} has Left\n");
        }
    }

}
