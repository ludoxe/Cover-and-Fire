using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Item/Gun")]
public class Data_Item_Gun : Data_Item
{
    [Header("Renderer")]
    [SerializeField] private bool TwoHandsGun ;
    [SerializeField] private Vector2 GunSpriteLocalPosition;
    [SerializeField] private Vector2 FirstHandGripPosition;
    [SerializeField] private Vector2 SecondHandGripPosition;

    [Header("Position Element")]
    [SerializeField] private Vector2 CanonPosition;
    [SerializeField] private Vector3 CanonRotation;

    [Header("Stats")]
    [SerializeField] private float WeaponBaseDamage = 0;



    public bool GetTwoHandsGun { get { return TwoHandsGun; } }
    public Vector2 GetGunSpriteLocalPosition {get { return GunSpriteLocalPosition; } }
    public Vector2 GetFirstHandGripPosition { get { return FirstHandGripPosition; } }
    public Vector2 GetSecondHandGripPosition 
    { 
        get 
        { 
            if(TwoHandsGun) return SecondHandGripPosition;
            else return FirstHandGripPosition;
        } 
    }

    public Vector2 GetCanonPosition { get { return CanonPosition; } }
    public Vector3 GetCanonRotation { get { return CanonRotation; } }

    public Utility.StructDamageInfo GetWeaponDamageStats
    {
        get
        {
            Utility.StructDamageInfo myStruct ;

            myStruct.DamageBase = WeaponBaseDamage;

            return myStruct;
        }
    }

}
