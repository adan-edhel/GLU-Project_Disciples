using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthbar
{
    void SetNametag(string name);
    void UpdateHealthbar(float currentHealth, float maxHealth);
}
