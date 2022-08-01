using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private float MaxHealth = 50;
    [SerializeField] private float health = 50;

    [SerializeField] private GameObject RagdollWhenDieRightOrientation;
    [SerializeField] private GameObject RagdollWhenDieLeftOrientation;

    [SerializeField] private GameObject Mainbone;



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

    public void SetHealth(float AmountHealth)
    {
        Health += AmountHealth;
    }

    #endregion



    private void UpdateEntityStatus()
    {
        if (health <= 0) StartCoroutine(Die());
        if (health > MaxHealth) health = MaxHealth;

    }



    private void SpawnRagdoll()
    {
        if (RagdollWhenDieRightOrientation == null || RagdollWhenDieLeftOrientation == null) return;

        var CharacterAppearance = GetComponent<CharacterAppearance>().GetExportedAppearance();

        GameObject RagdollPrefab = RagdollWhenDieRightOrientation;
        if (transform.rotation.eulerAngles.y == 180) RagdollPrefab = RagdollWhenDieLeftOrientation;

        Quaternion RagdollRotation = Quaternion.Euler(0, 0, Mainbone.transform.localRotation.eulerAngles.z);


         var Ragdoll = Instantiate(RagdollPrefab, Mainbone.transform.position, new Quaternion());

        Ragdoll.GetComponent<CharacterAppearance>().SetImportAppearanceSettings(CharacterAppearance);
        Ragdoll.transform.Find("Corpse/bone main corpse").localRotation = RagdollRotation;
    }

    private IEnumerator Die()
    {
        this.gameObject.SetActive(false);
        SpawnRagdoll();
        yield return null;
        Destroy(this.gameObject);
        
    }



}
