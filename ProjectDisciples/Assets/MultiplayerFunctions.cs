using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerFunctions : MonoBehaviourPunCallbacks
{
    public static MultiplayerFunctions Instance;
    private bool _quit;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(GetComponent<MultiplayerFunctions>());
        }
    }

    public void LeaveMultiplayerRoom()
    {
        _quit = false;
        DisconnectFromRoom();
    }

    public void QuitMultiplayerRoom()
    {
        _quit = true;
        DisconnectFromRoom();
    }

    private void DisconnectFromRoom()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(DisconectRoom(_quit));
    }

    private IEnumerator DisconectRoom(bool QuitGame = false)
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        if (QuitGame)
        {
            SceneController.Instance.QuitGame();
        }
        else if (!QuitGame)
        {
            SceneController.Instance.TransitionScene(0);
        }
    }
}
