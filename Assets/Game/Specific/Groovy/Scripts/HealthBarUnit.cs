using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarUnit : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Unit npc;

    [SerializeField] private Color lowHealth = new Color(r: 1, g: 0, b: 0), highHealth = new Color(r: 0, g: 1, b: 0);

    public void UpdateHealthBar()
    {
        float duration = 0.75f * (npc.HealthPrimantissa);
        healthBarImage.DOFillAmount(npc.HealthPrimantissa, duration);

        healthBarImage.color = Color.Lerp(lowHealth, highHealth, npc.HealthPrimantissa);
    }

    public void AdjustHealth(int _value)
    {
        npc.Health += _value;
        UpdateHealthBar();
    }
}
