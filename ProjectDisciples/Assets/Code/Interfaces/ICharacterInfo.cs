using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterInfo
{
    void SetNametag(string name);
    void UpdateHealthValue(float currentHealth, float maxHealth);
}