using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherecterAliveManeger : MonoBehaviourPunCallbacks
{
    public static CherecterAliveManeger Instance;
    [SerializeField] private List<IHealth> _cherecters;
    [SerializeField] private List<GameObject> _CherecterGameObjects;

    private void Awake()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine) return;
        Instance = this;
        _cherecters = new List<IHealth>();
        _CherecterGameObjects = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _cherecters.Count; i++)
        {
            Debug.Log(_cherecters[i]?.Health);
        }
    }

    public void addMe(IHealth Health, GameObject Gameobject)
    {
        _cherecters.Add(Health);
        _CherecterGameObjects.Add(Gameobject);
    }

    public void RemoveMe(IHealth Health, GameObject Gameobject)
    {
        _cherecters.Remove(Health);
        _CherecterGameObjects.Remove(Gameobject);
    }
}
