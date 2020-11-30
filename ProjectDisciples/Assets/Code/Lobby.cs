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
        
        string S = _Nickname.text;
        if (S == "")
        {
            S = RandomName(10);
        }

        PhotonNetwork.NickName = S;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(_ServerName.gameObject.name, this);
        string S = _ServerName.text;
        if (S == "")
        {
            S = RandomName(10);
        }
        print("connected");
        PhotonNetwork.JoinOrCreateRoom(S, new RoomOptions() { MaxPlayers = 5 , CleanupCacheOnLeave = true }, null);
    }

    public override void OnJoinedRoom()
    {
        print("joined room and waiting");
        SceneManager.LoadScene(1);
        DontDestroyOnLoad( PhotonNetwork.Instantiate(_PrefabLocation, Vector3.zero, Quaternion.identity));
        //_PunChat.ConnectToRoom(PhotonNetwork.CurrentRoom.Name,PhotonNetwork.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    private string RandomName(int Length)
    {
        string S = "";
        char[] Chars = "bcdfghjklmnpqrstvwxyz#&<>".ToCharArray();
        for (int i = 0; i < Length; i++)
        {
            S += Chars[Random.Range(0, (i * System.DateTime.Now.Millisecond)) % Chars.Length];
        }
        return S;
    }
}
