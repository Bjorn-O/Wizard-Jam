using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image manaBarImage;

    public void UpdateHealth(float health, float maxHealth)
    {
        healthBarImage.fillAmount = health / maxHealth;
        healthText.text = Mathf.RoundToInt(health) + "/" + maxHealth;
    }

    public void UpdateMana(float mana, float maxMana)
    {
        manaBarImage.fillAmount = mana / maxMana;
    }
}
