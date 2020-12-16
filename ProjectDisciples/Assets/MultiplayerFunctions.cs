using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerFunctions : MonoBehaviourPunCallbacks
{
    public static MultiplayerFunctions Instance;

    private void Start()
    {
        if (photonView.IsMine)
        {
            Instance = this;
        }
        else
        {
            Destroy(GetComponent<MultiplayerFunctions>());
        }
    }

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnLeftRoom()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.DestroyPlayerObjects(photonView.ViewID);
            Destroy(gameObject);
            SceneController.Instance.TransitionScene(0);
        }
    }
}
