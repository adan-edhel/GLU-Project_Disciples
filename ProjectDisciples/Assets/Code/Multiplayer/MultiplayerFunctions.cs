using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class MultiplayerFunctions : MonoBehaviourPunCallbacks
{
    public static MultiplayerFunctions Instance;
    [SerializeField] private GameObject _PlayerObserver;
    [SerializeField] private string _nickname;
    [SerializeField] private string _serverName;
    [SerializeField] private byte _maxPlayers;
    [SerializeField] private UnityEvent _updateNicknamePanel;
    private bool _quit;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            _updateNicknamePanel = new UnityEvent();
        }
        else
        {
            Destroy(GetComponent<MultiplayerFunctions>());
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region startGame / joinGame
    public void Multiplayer(string Nickname, string servername, byte maxplayer = 4, UnityAction UpdateNicknamePanel = null)
    {
        _nickname = Nickname; _serverName = servername; _maxPlayers = maxplayer;

        if (UpdateNicknamePanel != null)
        {
            _updateNicknamePanel.AddListener(UpdateNicknamePanel);
        }
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1.3";

        if (_nickname == "")
        {
            _nickname = RandomName(5);
        }
        PhotonNetwork.NickName = _nickname;
    }

    public override void OnConnectedToMaster()
    {
        string tempString = _serverName;
        if (tempString == "")
        {
            tempString = RandomName(5);
        }
        Debug.Log("Connected to master.", this);
        PhotonNetwork.JoinOrCreateRoom(tempString.ToUpper(), new RoomOptions() { MaxPlayers = _maxPlayers, CleanupCacheOnLeave = true }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined lobby & waiting...", this);
        PhotonNetwork.Instantiate("Character/PlayerObserver", Vector3.zero, Quaternion.identity);
        MatchManager.Instance.RegisterNickname(_nickname);
        _updateNicknamePanel.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Another player joined. name = {newPlayer.NickName}", this);
        MatchManager.Instance.RegisterNickname(newPlayer.NickName);
        _updateNicknamePanel.Invoke();
    }

    #endregion startGame / joinGame

    public void RemoveFromUpdateNicknamePanels(UnityAction Call)
    {
        _updateNicknamePanel.RemoveListener(Call);
    }

    #region Leaving game / quiting game
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

        _updateNicknamePanel.RemoveAllListeners();

        if (QuitGame)
        {
            SceneController.Instance.QuitGame();
        }
        else if (!QuitGame)
        {
            SceneController.Instance.TransitionScene(1);
        }
    }

    #endregion Leaving game / quiting game

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
