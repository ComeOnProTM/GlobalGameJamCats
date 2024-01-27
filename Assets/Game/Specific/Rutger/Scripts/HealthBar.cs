using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public Image healthBarImage;
    public Player player;

    [SerializeField] private Color lowHealth = new Color(r: 1, g: 0, b: 0), highHealth = new Color(r: 0, g: 1, b: 0);

    public void UpdateHealthBar()
    {
        float duration = 0.75f * (player.HealthPrimantissa);
        healthBarImage.DOFillAmount(player.HealthPrimantissa, duration);

        healthBarImage.color = Color.Lerp(lowHealth, highHealth, player.HealthPrimantissa);
        Debug.Log($"health primantissa: = health: {player.Health}, primantissa: {player.HealthPrimantissa}, max: {player.maxHealth}");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.Health -= 1;
            UpdateHealthBar();
        }
    }
}
