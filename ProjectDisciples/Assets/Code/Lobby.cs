using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _Nickname;
    [SerializeField] private TMP_InputField _ServerName;
    [SerializeField] private string _PrefabLocation;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void disconnect()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public void Multiplayer()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1.1";
        
        string tempString = _Nickname.text;
        if (tempString == "")
        {
            tempString = RandomName(10);
        }

        PhotonNetwork.NickName = tempString;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(_ServerName.gameObject.name, this);
        string tempString = _ServerName.text;
        if (tempString == "")
        {
            tempString = RandomName(10);
        }
        print("connected");
        PhotonNetwork.JoinOrCreateRoom(tempString, new RoomOptions() { MaxPlayers = 5 , CleanupCacheOnLeave = true }, null);
    }

    public override void OnJoinedRoom()
    {
        print("joined room and waiting");
        PUNChat.instance.ConnectToRoom(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName);
        //_PunChat.ConnectToRoom(PhotonNetwork.CurrentRoom.Name,PhotonNetwork.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    private string RandomName(int Length)
    {
        string tempString = "";
        char[] Chars = "bcdfghjklmnpqrstvwxyz#&<>".ToCharArray();
        for (int i = 0; i < Length; i++)
        {
            tempString += Chars[Random.Range(0, (i * System.DateTime.Now.Millisecond)) % Chars.Length];
        }
        return tempString;
    }
}
