using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    float Health { get; set; }

    void DealDamage(float Damage, EPlayerElement Element);
    
}
