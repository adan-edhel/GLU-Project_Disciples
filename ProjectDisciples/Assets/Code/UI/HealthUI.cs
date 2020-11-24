using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text desplayText;

    private float currentValue = 0f;
    [SerializeField] private float health;

    public float CurrentValue
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            slider.value = health;
            desplayText.text = (slider.value * 100).ToString() + " / 100";
        }
    }

    private void Start()
    {
        CurrentValue = 0f;
    }

    private void Update()
    {
        CurrentValue = health;
    }
}