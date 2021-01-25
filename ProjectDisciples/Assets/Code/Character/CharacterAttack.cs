﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EGameElement
{
    Fire = 0,
    Magma,
    Water,
    Ice,
    Wind,
    Earth,
    NoElement,
}

public class CharacterAttack : MonoBehaviourPunCallbacks, ICharacterElement, IMana
{
    [SerializeField] private List<EGameElement> _KnownElements;
    [SerializeField] private EGameElement _CurrentElement = EGameElement.Fire;

    [Header("mana")]
    [SerializeField] private float _maxMana = 300f;
    [SerializeField] private float _currentMana;
    [SerializeField] private float _regenPerFixedUpdete;
    [SerializeField] private float _firstAtackPrice;
    [SerializeField] private float _secondAtackPrice;

    [Header("renderer")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("aimer")]
    [SerializeField] private CharacterAim _characterAim;
    [SerializeField] private bool _canAttack;
 
    [Header("fire")]
    [SerializeField] private string _fireFirstAtackPath = "Elements/Fire/BaseAtack";
    [SerializeField] private Vector2 _firevelocity;
    [SerializeField] private float _firstFireAttackLifespan = 5;
    [SerializeField] private string _fireSecondAttackPath;

    [Header("Water")]
    [SerializeField] private string _waterFirstAtackPath = "Elements/Water/BaseAtack";
    [SerializeField] private Vector2 _watervelocity;
    [SerializeField] private float _firstWaterAttackLifespan = 5;
    [SerializeField] private string _waterSecondAttackPath;

    private void Start()
    {
        if (_KnownElements == null)
        {
            _KnownElements = new List<EGameElement>();
            _currentMana = _maxMana;
        }
    }

    private void FixedUpdate()
    {
        _currentMana += _regenPerFixedUpdete;
        _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);
    }

    public bool CanAttack
    {
        set
        {
            _canAttack = value;
            if (!_canAttack)
            {
                Color TempColor = _spriteRenderer.color;
                TempColor.a = 0.5f;
                _spriteRenderer.color = TempColor;
                return;
            }
            else if (_canAttack)
            {
                Color TempColor = _spriteRenderer.color;
                TempColor.a = 1f;
                _spriteRenderer.color = TempColor;
            }
        }
    }

    /// <summary>
    /// Sets the player element to the given element.
    /// </summary>
    /// <param name="Element"></param>
    /// <param name="AddToKnownElements">adds it to the known elements if it isn't in it.</param>
    public void SetElement(EGameElement Element, bool AddToKnownElements = false)
    {
        if (!_KnownElements.Contains(Element) && AddToKnownElements)
        {
            AddKnownElement = Element;
        }

        if (_KnownElements.Contains(Element))
        {
            _CurrentElement = Element;
        }
        else
        {
            Debug.LogError("element not in List", this);
        }
    }

    /// <summary>
    /// sets the player element to a Random element.
    /// </summary>
    /// <param name="AddToKnownElements">adds it to the known elements if it isn't in it.</param>
    public void SetElement(bool AddToKnownElements = false)
    {
        int MaxLength = System.Enum.GetValues(typeof(EGameElement)).Length;
        int I = UnityEngine.Random.Range(0, MaxLength * System.DateTime.Now.Millisecond) % MaxLength;
        SetElement((EGameElement)I, AddToKnownElements);
    }

    /// <summary>
    /// adds a new element to the known element List.
    /// </summary>
    public EGameElement AddKnownElement
    {
        set
        {
            _KnownElements.Add(value);
        }
    }

    public float MaxMana { get => _maxMana; }

    public float CurrentMana { get => _currentMana;}

    public void ResetMana()
    {
        _currentMana = _maxMana;
    }

    public void Attack1()
    {
        if (photonView == null && !photonView.IsMine && PhotonNetwork.InRoom) return;

        if (_currentMana >= _firstAtackPrice)
        {
            _currentMana -= _firstAtackPrice;
            switch (_CurrentElement)
            {
                case EGameElement.Fire:
                    FireAttack1();
                    break;
                case EGameElement.Water:
                    WaterAttack1();
                    break;
                case EGameElement.Wind:
                    WindAttack1();
                    break;
                case EGameElement.Earth:
                    EarthAttack1();
                    break;
            }
        }
    }

    public void Attack2()
    {
        if (photonView == null && !photonView.IsMine && PhotonNetwork.InRoom) return;
        if (_currentMana >= _secondAtackPrice)
        {
            _currentMana -= _secondAtackPrice;
            switch (_CurrentElement)
            {
                case EGameElement.Fire:
                    FireAttack2();
                    break;
                case EGameElement.Water:
                    WaterAttack2();
                    break;
                case EGameElement.Wind:
                    WindAttack2();
                    break;
                case EGameElement.Earth:
                    EarthAttack2();
                    break;
            }
        }
    }

    public void SwitchPreviousElement()
    {
        if (photonView == null && !photonView.IsMine && PhotonNetwork.InRoom && _KnownElements.Count == 0) return;
        int Current = (int)_CurrentElement;
        int Enumength = System.Enum.GetNames(typeof(EGameElement)).Length;
        for (int i = 1; i < Enumength; i++)
        {
            Current -= 1;
            if (Current < 0)
            {
                Current = Enumength + Current;
            }
            if (_KnownElements.Contains((EGameElement)Current))
            {
                SetElement((EGameElement)Current);
                break;
            }
        }
    }

    public void SwitchNextElement()
    {
        if (photonView == null && !photonView.IsMine && PhotonNetwork.InRoom && _KnownElements.Count == 0) return;
        int Current = (int)_CurrentElement;
        int Enumength = System.Enum.GetNames(typeof(EGameElement)).Length;
        for (int i = 1; i < Enumength; i++)
        {
            Current = (Current + 1) % Enumength;
            if (_KnownElements.Contains((EGameElement)Current))
            {
                SetElement((EGameElement)Current);
                break;
            }
        }
    }

    private Vector2 VelocityCalculater(Vector2 StartVelocity)
    {
        Vector2 NewVelocity = Vector2.zero;
        if (_characterAim != null)
        {
            NewVelocity = StartVelocity * _characterAim.aimDirection;
        }
        else if(_characterAim == null)
        {
            NewVelocity = new Vector2(StartVelocity.x * (_spriteRenderer.flipX ? -1 : 1), StartVelocity.y);
        }
        return NewVelocity;
    }

    #region Fire

    private void FireAttack1()
    {
        GameObject tempObject;
        if (PhotonNetwork.InRoom && photonView.IsMine)
        {
            tempObject = PhotonNetwork.Instantiate(_fireFirstAtackPath, transform.position, Quaternion.identity);
        }
        else
        {
            tempObject = Instantiate(Resources.Load(_fireFirstAtackPath) as GameObject, transform.position, Quaternion.identity);
        }

        tempObject.layer = gameObject.layer;

        Rigidbody2D RB = tempObject.GetComponent<Rigidbody2D>();
        if (RB != null)
        {
            RB.velocity = VelocityCalculater(_firevelocity);

        }

        Objectile objectileDamage = tempObject.GetComponent<Objectile>();
        if (objectileDamage != null)
        {
            objectileDamage.SetSender = this.gameObject;
            if (PhotonNetwork.InRoom && photonView.IsMine)
            {
                objectileDamage.SendRPC();
            }
            else
            {
                Destroy(tempObject, _firstFireAttackLifespan);
            }
        }
    }

    private void FireAttack2()
    {

    }

    #endregion Fire

    #region Water
    private void WaterAttack1()
    {
        GameObject tempObject;
        if (PhotonNetwork.InRoom)
        {
            tempObject = PhotonNetwork.Instantiate(_waterFirstAtackPath, transform.position, Quaternion.identity);
        }
        else
        {
            tempObject = Instantiate(Resources.Load(_waterFirstAtackPath) as GameObject, transform.position, Quaternion.identity);
        }
        tempObject.layer = gameObject.layer;

        Rigidbody2D RB = tempObject.GetComponent<Rigidbody2D>();
        if (RB != null)
        {
            RB.velocity = VelocityCalculater(_watervelocity);
        }

        Objectile objectileDamage = tempObject.GetComponent<Objectile>();
        if (objectileDamage != null)
        {
            objectileDamage.SetSender = this.gameObject;
        }
        Destroy(tempObject,_firstWaterAttackLifespan);
    }

    private void WaterAttack2()
    {

    }

    #endregion Water

    #region Wind

    private void WindAttack1()
    {

    }

    private void WindAttack2()
    {

    }

    #endregion wind

    #region Earth

    private void EarthAttack1()
    {

    }

    private void EarthAttack2()
    {

    }
    #endregion Eurth
}


