using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private float MaxHealth = 50;
    [SerializeField] private float Health = 50;

    #region interface

    public void ReceiveDamage(Utility.StructDamageInfo DamageInfo)
    {
        SetHealth(DamageInfo.DamageBase * -1);

        print(this.gameObject.name + " Received Damages");
    }

    #endregion

    public void SetHealth(float AmountHealth)
    {
        Health += AmountHealth;
    }

    private void UpdateEntityStatus()
    {
        if (Health <= 0) Die();
        if (Health > MaxHealth) Health = MaxHealth;

    }





    private void Die()
    {

    }



}
