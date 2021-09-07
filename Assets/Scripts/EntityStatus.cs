using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private float MaxHealth = 50;
    [SerializeField] private float health = 50;

    private float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            UpdateEntityStatus();
        }
    }

    #region interface

    public void ReceiveDamage(Utility.StructDamageInfo DamageInfo)
    {
        SetHealth(DamageInfo.DamageBase * -1);

    }

    #endregion

    public void SetHealth(float AmountHealth)
    {
        Health += AmountHealth;
    }

    private void UpdateEntityStatus()
    {
        if (health <= 0) Die();
        if (health > MaxHealth) health = MaxHealth;

    }





    private void Die()
    {
        print("Dying");
    }



}
