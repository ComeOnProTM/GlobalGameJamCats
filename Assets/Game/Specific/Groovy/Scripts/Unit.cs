using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;

public class Unit : MonoBehaviour
{
    [Header("NPC Stats")]
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask fireLayer;

    [Header("Health")]
    [SerializeField] private int healthBackingField = 20;
    [SerializeField] private float damageTimerMax = 2f;
    [SerializeField] private HealthBar healthBar;
    private bool damageTick;
    private float damageTimer;
    private float healthSpeedModifier;
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private float damageSpeedModifier = 1f;
    [SerializeField] private float healSpeedModifier = 3f;
    private float currentHealth;
    public int Health
    {
        get => healthBackingField;
        set
        {
            healthBackingField = clamp(value, 0, maxHealth);
        }
    }

    public float HealthPrimantissa => (float)Health / (float)maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        NpcManager.Instance.AddNPC(gameObject);
    }

    private void OnDestroy()
    {
        NpcManager.Instance.RemoveNPC(gameObject);
    }

    private void Update()
    {
        // Start is called before the first frame update
        if (damageTick)
        {
            damageTimer -= Time.deltaTime * healthSpeedModifier;
            if (damageTimer < 0)
            {
                damageTick = false;
                damageTimer = damageTimerMax;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, fireLayer);
        Debug.Log(hitColliders.Length);
        if (hitColliders.Length > 0)
        {
            foreach (Collider _collider in hitColliders)
            {
                if (!damageTick && _collider.TryGetComponent<SmallFireScript>(out SmallFireScript _))
                {
                    currentHealth -= 1;
                    healthBar.AdjustHealth(-1);
                    damageTick = true;
                    healthSpeedModifier = damageSpeedModifier;
                    break;
                }
            }
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
