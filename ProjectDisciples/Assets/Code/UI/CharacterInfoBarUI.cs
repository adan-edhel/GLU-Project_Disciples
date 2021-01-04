using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterInfoBarUI : MonoBehaviour, ICharacterInfo
{
    [SerializeField] TextMeshProUGUI Nametag;
    [SerializeField] Image Healthbar;

    [SerializeField] private float HPLerpSpeed = 2;

    [Range(0, 1), SerializeField] private float manaPercentage;
    [Range(0, 1), SerializeField] private float healthPercentage;

    private void LateUpdate()
    {
        if (Healthbar.fillAmount != healthPercentage)
        {
            Healthbar.fillAmount = Mathf.Lerp(Healthbar.fillAmount, healthPercentage, HPLerpSpeed / 100);
        }
    }

    public void SetNametag(string name)
    {
        Nametag.text = name;
    }

    public void UpdateManaValue(float currentMana, float maxMana)
    {
        manaPercentage = currentMana / maxMana;
    }

    public void UpdateHealthValue(float currentHealth, float maxHealth)
    {
        healthPercentage = currentHealth / maxHealth;
    }
}
