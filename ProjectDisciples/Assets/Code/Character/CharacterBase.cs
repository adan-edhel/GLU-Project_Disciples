using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CharacterBase : MonoBehaviourPunCallbacks, IHealth
{
    public IOnPlayerDeath OnPlayerDeath;
    ICharacterInfo _characterInfo;

    private const float _maxHealth = 300;
    [Range(0, 300), SerializeField] private float _health = 300;

    [SerializeField] private float _MaxStatesEfectTime;
    [SerializeField] private CharacterAttack _CharacterAttack;
    [SerializeField] private Dictionary<int, float> _statesEfects;
    [SerializeField] private ElementInteraction[] _elementInteractions;

    private void Start()
    {
        _statesEfects = new Dictionary<int, float>();

        MatchManager.Instance?.RegisterCharacter(this, gameObject);

        _characterInfo = GetComponentInChildren<ICharacterInfo>();

        _characterInfo.SetNametag(photonView.Owner.NickName);
        _characterInfo.UpdateHealthValue(_health, _maxHealth);

    }

    private void Update()
    {
        //TODO: Move to Deal Damage and remove it from here
        _characterInfo.UpdateHealthValue(_health, _maxHealth);

        if (PhotonNetwork.InRoom && photonView.IsMine)
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
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health == -1f) _CharacterAttack.CanAttack = false;
            else _CharacterAttack.CanAttack = true;
        }
    }

    private void OnDestroy()
    {
        MatchManager.Instance?.RemoveCharacter(this, gameObject);
    }

    public void DealDamage(float Damage, EGameElement Element)
    {
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC("NetworkDealDamage", RpcTarget.Others, Damage, Element);
        }

        NetworkDealDamage(Damage, Element);

    }

    [PunRPC]
    public void NetworkDealDamage(float Damage, EGameElement Element)
    {
        if (_health > 0)
        {
            CheckIfPlayerHasStatesEfect(Element);
            Damage *= Multiplier;
            _health -= Damage;
            if (_health <= 0 && photonView.IsMine)
            {
                OnPlayerDeath?.OnPlayerDeath();
                PhotonNetwork.Destroy(photonView);
            }
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            _characterInfo.UpdateHealthValue(_health, _maxHealth);
        }
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

    public PhotonView GetPhotonView
    {
        get { return photonView; }
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

    public void ResetHealth()
    {
        photonView.RPC("NetworkReset", RpcTarget.All);
    }
    
    [PunRPC]
    public void NetworkReset()
    {
        _health = _maxHealth;
    }
}
