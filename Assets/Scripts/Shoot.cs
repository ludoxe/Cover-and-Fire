using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK ;

public class Shoot : MonoBehaviour
{
    [Header("Weapon")]
    private Data_Item_Gun WeaponData;
    [SerializeField] private GameObject WeaponGameObject;
    [SerializeField] private GameObject BulletWeaponLineCache;
    [SerializeField] private GameObject WeaponCanon ;

    [Header("Weapon Placement")]
    [SerializeField] private GameObject FirstHandGripGameObject;
    [SerializeField] private GameObject SecondHandGripGameObject;
    [SerializeField]private GameObject BoneLeftArmDown;
    [SerializeField] private GameObject BoneLeftArmUp;
    private GameObject BoneForAim { get { if (WeaponData == null) return null; else if (WeaponData.GetTwoHandsGun) return BoneLeftArmDown; else return BoneLeftArmUp; } }

    [Header("Arm Target")]
    [SerializeField] private GameObject LeftArmTarget;
    [SerializeField] private GameObject RightArmTarget;

    [Header("Aim Parameter")]
    [SerializeField] private GameObject AimReferencePoint;
    private float AngleBetweenEntityAndWeaponCanon;

    [Header("Other")]
    [SerializeField] private GameObject HeadBone;
    [SerializeField] private GameObject Target;

    public Data_Item_Gun SetWeapon
    { 
        set
        {
            WeaponData = value;
            UpdateScript();
        }
    }

    private void Start()
    {
        ExternalSetStatePhaseEntityStructVariables();
    }
    private void ExternalSetStatePhaseEntityStructVariables()
    {
        var AimStruct = new StatePhaseEntity.StructAimVariables();

        AimStruct.LeftArmTarget = LeftArmTarget;
        AimStruct.RightArmTarget = RightArmTarget;
        AimStruct.FirstHandGripGameObject = FirstHandGripGameObject;
        AimStruct.SecondHandGripGameObject = SecondHandGripGameObject;
        AimStruct.AimReferencePoint = AimReferencePoint;


        var ShootStruct = new StatePhaseEntity.StructShootVariables();

        ShootStruct.WeaponCanon = WeaponCanon;
        ShootStruct.BulletWeaponLineCache = BulletWeaponLineCache;

        GetComponent<StatePhaseEntity>().AimVariables = AimStruct;
        GetComponent<StatePhaseEntity>().ShootVariables = ShootStruct;
    }
    private void UpdateScript() 
    {
        if (WeaponData == null) return;

        FirstHandGripGameObject.transform.localPosition = WeaponData.GetFirstHandGripPosition;
        SecondHandGripGameObject.transform.localPosition = WeaponData.GetSecondHandGripPosition;
        WeaponCanon.transform.localPosition = WeaponData.GetCanonPosition;
        WeaponCanon.transform.localRotation = Quaternion.Euler(WeaponData.GetCanonPosition);
    }

}
