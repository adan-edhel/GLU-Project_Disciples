using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PUNChat : MonoBehaviour, IChatClientListener
{
    [SerializeField] private string _Nickname;
    [SerializeField] private Text _textField;
    string _textfieldname;
    public Text _writefield;
    string _writeFieldName;
    [SerializeField] private string _RoomName;
 
    private ChatClient _chatclient;
    public static PUNChat instance;

    #region Unity Methods
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        _textfieldname = _textField.gameObject.name;
        _writeFieldName = _writefield.gameObject.name;
    }
    private void Start()
    {
        _chatclient = new ChatClient(this);
        ConnectToPUNChat();
    }

    private void Update()
    {
        _chatclient.Service();
    }
    #endregion Unity Methods

    #region function
    public void ConnectToPUNChat()
    {
        Debug.Log("connecting to chat");
        _chatclient.AuthValues = new Photon.Chat.AuthenticationValues(_Nickname);
        ChatAppSettings chatSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        _chatclient.ConnectUsingSettings(chatSettings);
    }

    public void ConnectToRoom(string room, string Nickname)
    {
        UnsubscribeRoom(new string[] { _RoomName });
        _RoomName = room;
        _Nickname = Nickname;
        SubscribeRoom(new string[] { _RoomName });
    }

    public void DisconnectFromRoom()
    {
        UnsubscribeRoom(new string[] { _RoomName });
    }

    #endregion function

    #region Public functions
    public void sendmessage()
    {
        SendRoomMessage(_RoomName, $"{_Nickname}: {_writefield.text}");
        _writefield.text = "";
    }

    public void SendPrivedMessage(string recepient, string message)
    {
        _chatclient.SendPrivateMessage(recepient, message);
    }

    public void SubscribeRoom(string[] chennels)
    {
        _chatclient.Subscribe(chennels);
    }

    public void UnsubscribeRoom(string[] chennels)
    {
        _chatclient.Unsubscribe(chennels);
    }

    public void SendRoomMessage(string Chennel, string message)
    {
        _chatclient.PublishMessage(Chennel, message);
    }

    #endregion

    #region chatcalbacks
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnDisconnected()
    {
        print("you are Disconnected to chat");
    }

    public void OnConnected()
    {
        print("you are connected to chat");
    }

    public void OnChatStateChange(ChatState state)
    {
      
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            if (!string.IsNullOrEmpty(messages[i].ToString()))
            {
                // chennel name format [sender : recipient]
                string[] splitname = channelName.Split(':');
                string sendername = splitname[0];
                if (!senders[i].Equals(sendername, StringComparison.OrdinalIgnoreCase))
                {
                    if (_textField == null)
                    {
                        GameObject T = GameObject.Find(_textfieldname);
                        if (T != null)
                        {
                            _textField = T.GetComponent<Text>();
                        }
                        T = GameObject.Find(_writeFieldName);
                        if (T != null)
                        {
                            _writefield = T.GetComponent<Text>();
                        }
                    }

                    if (_textField != null)
                    {
                        _textField.text += ($"\n{messages[i]}");
                    }
                }
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            string[] splitname = channelName.Split(':');
            string sendername = splitname[0];
            if (!sender.Equals(sendername,StringComparison.OrdinalIgnoreCase))
            {
                _textField.text += ($"\n{message}");
            }
        }
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (results[i])
            {
                //SendRoomMessage(channels[i], $"{_Nickname} has joined the game");
                GameObject G = PhotonNetwork.Instantiate("Player/Player", Vector3.zero, Quaternion.identity);
                G.name = _Nickname;
                //if (G.GetComponent<PhotonView>().IsMine)
                //{
                //    Gamemaneger.Instanse.setViewer = G.GetComponent<PhotonView>();
                //}
            }
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            SendRoomMessage(channels[i], $"{_Nickname} has Left the game");
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
      
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
       
    }
    #endregion
}
