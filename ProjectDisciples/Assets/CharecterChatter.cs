using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharecterChatter : MonoBehaviourPunCallbacks
{
    [SerializeField] private ChatterPanel _textPanel;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            _textPanel.SetAsStatic();
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
        photonView.RPC("SetMessage", RpcTarget.All, Message);
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
