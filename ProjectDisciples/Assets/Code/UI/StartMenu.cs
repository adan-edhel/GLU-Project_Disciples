using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class StartMenu : MonoBehaviourPunCallbacks
{
    private RoomOptions roomOptions = new RoomOptions();

    [SerializeField] private Button startMultiButton;
    [SerializeField] private Button quitButton;
    //[SerializeField] private TMP_InputField username;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        startMultiButton.onClick.AddListener(UIStartMultiBut);
        quitButton.onClick.AddListener(UIQuitBut);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public void Connect()
    {
        if (PhotonNetwork.JoinOrCreateRoom("Test", roomOptions, null))
        {
            Debug.Log("Room joined");
        }
    }

    private void UIStartMultiBut()
    {
        //PhotonNetwork.NickName = username.text;

        Connect();
    }

    private void UIQuitBut()
    {
        Application.Quit();
    }
}