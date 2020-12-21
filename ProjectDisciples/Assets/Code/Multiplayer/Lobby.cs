using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Lobby : MonoBehaviour
{
    [SerializeField] private TMP_InputField _Nickname;
    [SerializeField] private TMP_InputField _ServerName;
    [SerializeField] private TMP_Text _playerNicknames;
    [SerializeField] private TMP_Text _ServerNameTextField;
    [SerializeField] private Button _button;
    //[SerializeField] private string _PrefabLocation;

    private void Start()
    {
#if !UNITY_EDITOR
                _button.interactable = false;
#endif
    }

    public void Multiplayer()
    {
        MultiplayerFunctions.Instance.Multiplayer(_Nickname.text, _ServerName.text, UpdateNicknamePanel: updateNicknamePanel);
    }

    private void updateNicknamePanel()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            print("Player count is: " + (int)PhotonNetwork.CurrentRoom.PlayerCount);
            if (((int)PhotonNetwork.CurrentRoom.PlayerCount) >= 2)
            {
                _button.interactable = true;
            }
            else
            {
#if !UNITY_EDITOR
                _button.interactable = false;
#endif
            }
        }
        _ServerNameTextField.text = PhotonNetwork.CurrentRoom.Name;
        _playerNicknames.text = "";
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            _playerNicknames.text += (player.Value.NickName + "\n");
        }
    }
}