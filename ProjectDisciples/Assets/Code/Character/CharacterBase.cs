using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CharacterBase : MonoBehaviourPunCallbacks, IHealth, IMana
{
    public IOnPlayerDeath OnPlayerDeath;
    ICharacterInfo _characterInfo;

    private const float _maxHealth = 300;
    [Range(0, 300), SerializeField] private float _health = 300;

    [SerializeField] private float _MaxStatesEfectTime;
    [SerializeField] private CharacterAttack _CharacterAttack;
    [SerializeField] private Dictionary<int, float> _statesEffects;
    [SerializeField] private ElementInteraction[] _elementInteractions;
    [Header("mana")]
    [SerializeField] private float _currentMana;
    [SerializeField] private float _maxMana = 300f;
    [SerializeField] private float _regenPerFixedUpdete;

    private void Start()
    {
        _statesEffects = new Dictionary<int, float>();

        MatchManager.Instance?.RegisterCharacter(this, gameObject);
        _currentMana = _maxMana;
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
                if (_statesEffects.ContainsKey(i))
                {
                    _statesEffects[i] -= Time.fixedDeltaTime;
                    if (_statesEffects[i] <= 0)
                    {
                        _statesEffects.Remove(i);
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

    public float MaxHealth { get => _maxHealth; }

    public float CurrentMana { get => _currentMana;
        set { _currentMana = value; }
    }
    
    public float MaxMana { get => _maxMana; }

    public void ResetMana()
    {
        _currentMana = _maxMana;
    }

    private void FixedUpdate()
    {
        _currentMana += _regenPerFixedUpdete;
        _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);
    }

    private void OnDestroy()
    {
        MatchManager.Instance?.RemoveCharacter(this, gameObject);
    }

    public void DealDamage(float Damage, EGameElement Element, string NicknameDamegeDealer)
    {
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC("NetworkDealDamage", RpcTarget.Others, Damage, Element, NicknameDamegeDealer);
        }

        NetworkDealDamage(Damage, Element, NicknameDamegeDealer);

    }

    [PunRPC]
    public void NetworkDealDamage(float Damage, EGameElement Element, string NicknameDamegeDealer)
    {
        if (_health > 0)
        {
            CheckIfPlayerHasStatesEfect(Element);
            Damage *= Multiplier(out EGameElement[] effects);
            _health -= Damage;
            if (_health <= 0 && photonView.IsMine)
            {
                OnPlayerDeath?.OnPlayerDeath();
                PhotonNetwork.Destroy(photonView);
                string Message = $"{NicknameDamegeDealer} has defeated {photonView.Owner.NickName} with ";
                if (effects.Length != 0)
                {
                    for (int i = 0; i < effects.Length; i++)
                    {
                        if (i != 0)
                        {
                            Message += " & ";
                        }
                        Message += $"<sprite={(int)effects[i]}>";
                    }
                }
                else
                {
                    Message += $"<sprite={(int)Element}>";
                }

                CharecterChatter.Instance.RPCSendDeathMessage(Message+"\n");
            }
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            _characterInfo.UpdateHealthValue(_health, _maxHealth);
        }
    }


    private void CheckIfPlayerHasStatesEfect(EGameElement Element)
    {
        if (!_statesEffects.ContainsKey((int)Element))
        {
            _statesEffects.Add((int)Element, _MaxStatesEfectTime);
        }
        else
        {
            _statesEffects[(int)Element] = _MaxStatesEfectTime;
        }
    }

    private float Multiplier(out EGameElement[] Effects)
    {
        float multiplier = 1f;
        List<EGameElement> elements = new List<EGameElement>();
        for (int i = 0; i < _elementInteractions.Length; i++)
        {
            if (_statesEffects.ContainsKey((int)_elementInteractions[i].GetFirstElement) && _statesEffects.ContainsKey((int)_elementInteractions[i].GetSecondElement))
            {
                multiplier *= _elementInteractions[i].GetMultplier;
                _statesEffects.Remove((int)_elementInteractions[i].GetFirstElement);
                _statesEffects.Remove((int)_elementInteractions[i].GetSecondElement);

                elements.Add(_elementInteractions[i].GetFirstElement);
                elements.Add(_elementInteractions[i].GetSecondElement);
            }
        }
        Effects = elements.ToArray();
        return multiplier;
    }

    public PhotonView GetPhotonView
    {
        get { return photonView; }
    }

    public bool HasStatusEffects 
    {
        get
        {
            if (_statesEffects.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_statesEffects);
            stream.SendNext(_health);
        }
        else if (stream.IsReading)
        {
            _statesEffects = (Dictionary<int, float>)stream.ReceiveNext();
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
