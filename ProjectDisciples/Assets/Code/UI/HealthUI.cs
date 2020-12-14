using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public Image healthUI;
    public TMP_Text desplayText;

    private float currentValue = 0f;

    private CharacterHealth characterHealth;

    public float CurrentValue
    {
        get
        {
            return characterHealth.Health;
        }
        set
        {
            characterHealth.Health = value;
            healthUI.fillAmount = characterHealth.Health / 300;
            desplayText.text = (healthUI.fillAmount * 300).ToString() + " / 300";
        }
    }

    private void Start()
    {
        characterHealth = FindObjectOfType<CharacterHealth>();

        CurrentValue = characterHealth.Health;
    }

    private void Update()
    {
        CurrentValue = characterHealth.Health;
    }
}