using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float _health;
    [SerializeField] private float _MaxStatesEfectTime;
    [SerializeField] private Dictionary<EGameElement, float> _statesEfects;
    [SerializeField] private ElementInteraction[] _elementInteractions;

    public float Health { get { return _health;} set { _health = value; } }

    public void DealDamage(float Damage, EGameElement Element)
    {
        CheckIfPlayerHasStatesEfect(Element);
        Damage *= Multiplier;
        _health -= Damage;
    }

    private void CheckIfPlayerHasStatesEfect(EGameElement Element)
    {
        if (!_statesEfects.ContainsKey(Element))
        {
            _statesEfects.Add(Element, _MaxStatesEfectTime);
        }
        else
        {
            _statesEfects[Element] = _MaxStatesEfectTime;
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
        _statesEfects = new Dictionary<EGameElement, float>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < Enum.GetNames(typeof(EGameElement)).Length; i++)
        {
            if (_statesEfects.ContainsKey((EGameElement)i))
            {
                _statesEfects[(EGameElement)i] -= Time.fixedDeltaTime;
                if (_statesEfects[(EGameElement)i] <= 0)
                {
                    _statesEfects.Remove((EGameElement)i);
                }
            }
        }
    }
}
