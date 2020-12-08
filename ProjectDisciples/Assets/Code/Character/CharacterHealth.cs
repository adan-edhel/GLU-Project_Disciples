using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviourPunCallbacks, IHealth
{
    [SerializeField] private float _health;
    [SerializeField] private float _MaxStatesEfectTime;
    [SerializeField] private Dictionary<int, float> _statesEfects;
    [SerializeField] private ElementInteraction[] _elementInteractions;

    public float Health { get { return _health;} set { _health = value; } }

    public void DealDamage(float Damage, EGameElement Element)
    {
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC("NetworkDealDamage", RpcTarget.Others, Damage, Element);
        }
        else
        {
            NetworkDealDamage(Damage, Element);
        }
    }

    [PunRPC]
    public void NetworkDealDamage(float Damage, EGameElement Element)
    {
        CheckIfPlayerHasStatesEfect(Element);
        Damage *= Multiplier;
        _health -= Damage;
    }


    private void CheckIfPlayerHasStatesEfect(EGameElement Element)
    {
        if (!_statesEfects.ContainsKey((int)Element))
        {
            _statesEfects.Add((int)Element, _MaxStatesEfectTime);
        }
        else
        {
            _statesEfects[(int)Element] = _MaxStatesEfectTime;
        }
    }

    private float Multiplier
    {
        get
        {
            float multiplier = 1f;
            for (int i = 0; i < _elementInteractions.Length; i++)
            {
                if (_statesEfects.ContainsKey((int)_elementInteractions[i].GetFirstElement) && _statesEfects.ContainsKey((int)_elementInteractions[i].GetSecondElement))
                {
                    multiplier *= _elementInteractions[i].GetMultplier;
                    _statesEfects.Remove((int)_elementInteractions[i].GetFirstElement);
                    _statesEfects.Remove((int)_elementInteractions[i].GetSecondElement);
                }
            }
            return multiplier;
        }
    }


    private void Start()
    {
        _statesEfects = new Dictionary<int, float>();
        CherecterAliveManeger.Instance?.addMe(this, gameObject);
    }

    private void FixedUpdate()
    {
        if(PhotonNetwork.InRoom && photonView.IsMine)
        {
            for (int i = 0; i < Enum.GetNames(typeof(EGameElement)).Length; i++)
            {
                if (_statesEfects.ContainsKey(i))
                {
                    _statesEfects[i] -= Time.fixedDeltaTime;
                    if (_statesEfects[i] <= 0)
                    {
                        _statesEfects.Remove(i);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        CherecterAliveManeger.Instance?.RemoveMe(this, gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_statesEfects);
            stream.SendNext(_health);
        }
        else if (stream.IsReading)
        {
            _statesEfects = (Dictionary<int, float>)stream.ReceiveNext();
            _health = (float)stream.ReceiveNext();
        }
    }
}
