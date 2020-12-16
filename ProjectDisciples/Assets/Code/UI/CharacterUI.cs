using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour, IHealthbar
{
    [SerializeField] TextMeshProUGUI Nametag;
    [SerializeField] Image Healthbar;

    [SerializeField] private float HPLerpSpeed = 2;

    [Range(0, 1), SerializeField] private float calculatePercentage;

    private void LateUpdate()
    {
        if (Healthbar.fillAmount != calculatePercentage)
        {
            Healthbar.fillAmount = Mathf.Lerp(Healthbar.fillAmount, calculatePercentage, HPLerpSpeed / 100);
        }
    }

    public void SetNametag(string name)
    {
        Nametag.text = name;
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth)
    {
        calculatePercentage = currentHealth / maxHealth;
    }
}
