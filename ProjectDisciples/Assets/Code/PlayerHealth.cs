using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float _health;
    [SerializeField] private float _MaxStatesEfect;
    [SerializeField] private Dictionary<EPlayerElement, float> _statesEfects;
    [SerializeField] private ElementInteraction[] _elementInteractions;

    public float Health { get { return _health;} set { _health = value; } }

    public void DealDamage(float Damage, EPlayerElement Element)
    {
        CheckIfPlayerHasStatesEfect(Element);
        Damage *= Multiplier;
        _health -= Damage;
    }

    private void CheckIfPlayerHasStatesEfect(EPlayerElement Element)
    {
        if (!_statesEfects.ContainsKey(Element))
        {
            _statesEfects.Add(Element, _MaxStatesEfect);
        }
        else
        {
            _statesEfects[Element] = _MaxStatesEfect;
        }
    }

    private float Multiplier
    {
        get
        {
            float multiplier = 1f;
            for (int i = 0; i < _elementInteractions.Length; i++)
            {
                if (_statesEfects.ContainsKey(_elementInteractions[i].GetFirstElement) && _statesEfects.ContainsKey(_elementInteractions[i].GetSecondElement))
                {
                    multiplier *= _elementInteractions[i].GetMultplier;
                    _statesEfects.Remove(_elementInteractions[i].GetFirstElement);
                    _statesEfects.Remove(_elementInteractions[i].GetSecondElement);
                }
            }
            return multiplier;
        }
    }


    private void Start()
    {
        _statesEfects = new Dictionary<EPlayerElement, float>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < Enum.GetNames(typeof(EPlayerElement)).Length; i++)
        {
            if (_statesEfects.ContainsKey((EPlayerElement)i))
            {
                _statesEfects[(EPlayerElement)i] -= Time.fixedDeltaTime;
                if (_statesEfects[(EPlayerElement)i] <= 0)
                {
                    _statesEfects.Remove((EPlayerElement)i);
                }
            }
        }
    }
}
