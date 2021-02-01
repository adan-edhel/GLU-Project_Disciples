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
    [SerializeField] private int _maxPlayers;
    [SerializeField] private TMP_Text _maxPlayerText;
    [SerializeField] private Button _startButton;

    [SerializeField] private CharecterColors _charecterColors;
    private void Start()
    {
#if !UNITY_EDITOR
                _button.interactable = false;
#endif
    }

    public void Multiplayer()
    {
        MultiplayerFunctions.Instance.Multiplayer(_Nickname.text, _ServerName.text, (byte)_maxPlayers, UpdateNicknamePanel: updateNicknamePanel);
    }

    private void updateNicknamePanel()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            print("Player count is: " + (int)PhotonNetwork.CurrentRoom.PlayerCount);
            if (((int)PhotonNetwork.CurrentRoom.PlayerCount) >= 2)
            {
                _startButton.interactable = true;
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

        for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            _playerNicknames.text += $"<color=#{ColorUtility.ToHtmlStringRGB(_charecterColors.getColors[i])}> {PhotonNetwork.CurrentRoom.Players[1 + i].NickName} </color>\n";
        }

    }

    public void UpdateMaxPlayer(float maxplayer)
    {
        _maxPlayers = (int)maxplayer;
        _maxPlayerText.text = _maxPlayers.ToString(); 
    }

    public void LeaveLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            MultiplayerFunctions.Instance.LeaveMultiplayerRoom();
        }
    }

    private void OnDestroy()
    {
        MultiplayerFunctions.Instance.RemoveFromUpdateNicknamePanels(updateNicknamePanel);
    }
}