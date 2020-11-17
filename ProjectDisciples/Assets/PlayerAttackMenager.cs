using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMenager : MonoBehaviour
{
    [SerializeField] private  _firstAttack;

    public void FerstAttack()
    {
        ((IPlayerAttack)_firstAttack).Attack();
    }
}
