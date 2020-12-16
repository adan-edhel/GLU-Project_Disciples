using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _Nickname;
    [SerializeField] private TMP_InputField _ServerName;
    [SerializeField] private TMP_Text _playerNicknames;
    [SerializeField] private TMP_Text _ServerNameTextField;
    [SerializeField] private Button _button;
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
            tempString = RandomName(5);
        }

        PhotonNetwork.NickName = tempString;
    }

    public override void OnConnectedToMaster()
    {
        string tempString = _ServerName.text;
        if (tempString == "")
        {
            tempString = RandomName(5);
        }
        _ServerNameTextField.text = tempString.ToUpper();

        print("Connected to master.");
        PhotonNetwork.JoinOrCreateRoom(tempString.ToUpper(), new RoomOptions() { MaxPlayers = 5 , CleanupCacheOnLeave = true }, null);
    }

    public override void OnJoinedRoom()
    {
        print("Joined lobby & waiting...");
        PUNChat.instance.ConnectToRoom(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.NickName);

        updateNicknamePanel();
        //_PunChat.ConnectToRoom(PhotonNetwork.CurrentRoom.Name,PhotonNetwork.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        updateNicknamePanel();
    }

    private void updateNicknamePanel()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            print("Player count is: " + (int)PhotonNetwork.CurrentRoom.PlayerCount);
            if (((int)PhotonNetwork.CurrentRoom.PlayerCount) >= 2)
            {
                //_button.interactable = true;
            }
            else
            {
                //_button.interactable = false;
            }
        }

        _playerNicknames.text = "";
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            _playerNicknames.text += (player.Value.NickName + "\n");
        }
    }

    private string RandomName(int Length)
    {
        string tempString = "";
        char[] Chars = "bcdfghjklmnpqrstvwxyz#&<>1234567890".ToCharArray();
        for (int i = 0; i < Length; i++)
        {
            tempString += Chars[Random.Range(0, (i * System.DateTime.Now.Millisecond)) % Chars.Length];
        }
        return tempString;
    }
}