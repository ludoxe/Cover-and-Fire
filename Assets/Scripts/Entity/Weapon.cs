using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK ;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    private Data_Item_Gun GunData;
    private Data_Item_MeleeWeapon MeleeWeaponData;
    [SerializeField] private GameObject WeaponGameObject;
    [SerializeField] private GameObject BulletWeaponLineCache;
    [SerializeField] private GameObject WeaponCanon ;

    [Header("Gun Placement")]
    [SerializeField] private GameObject FirstHandGripGunGameObject;
    [SerializeField] private GameObject SecondHandGripGunGameObject;
    [SerializeField]private GameObject BoneLeftArmDown;
    [SerializeField] private GameObject BoneLeftArmUp;
    private GameObject BoneForAim { get { if (GunData == null) return null; else if (GunData.GetTwoHandsGun) return BoneLeftArmDown; else return BoneLeftArmUp; } }

    [Header("Melee Weapon Placement")]
    [SerializeField] private GameObject FirstHandGripMeleeGameObject;
    [SerializeField] private GameObject SecondHandGripMeleeGameObject;

    [Header("Arm Target")]
    [SerializeField] private GameObject LeftArmTarget;
    [SerializeField] private GameObject RightArmTarget;

    [Header("Aim Parameter")]
    [SerializeField] private GameObject AimReferencePoint;
    private float AngleBetweenEntityAndWeaponCanon;

    [Header("Default Melee Weapon")]

    [SerializeField] private Data_Item_MeleeWeapon DefaultMeleeWeaponData;

    [Header("Cache")]
    private Data_Item_Gun PreviousGunWeaponData;
    private Data_Item_MeleeWeapon PreviousMeleeWeaponData;

    [Header("Other")]
    [SerializeField] private GameObject HeadBone;
    [SerializeField] private GameObject Target;


    public Data_Item_Gun SetGunWeapon
    { 
        set
        {
            PreviousGunWeaponData = GunData;
            GunData = value;
            UpdateScriptForGun();
        }
    }
    public Data_Item_MeleeWeapon SetMeleeWeapon
    {
        set
        {
            PreviousMeleeWeaponData = MeleeWeaponData;
            MeleeWeaponData = value;
            UpdateScriptForMeleeWeapon();
        }

    }
    private void Start()
    {
        ExternalSetStatePhaseEntityStructVariables();
    }
    private void ExternalSetStatePhaseEntityStructVariables()
    {
        var AimStruct = new Utility.StructAimVariables();

        AimStruct.LeftArmTarget = LeftArmTarget;
        AimStruct.RightArmTarget = RightArmTarget;
        AimStruct.FirstHandGripGameObject = FirstHandGripGunGameObject;
        AimStruct.SecondHandGripGameObject = SecondHandGripGunGameObject;
        AimStruct.AimReferencePoint = AimReferencePoint;


        var ShootStruct = new Utility.StructShootVariables();

        ShootStruct.WeaponCanon = WeaponCanon;
        ShootStruct.BulletWeaponLineCache = BulletWeaponLineCache;

        GetComponent<StatePhaseEntity>().AimVariables = AimStruct;
        GetComponent<StatePhaseEntity>().ShootVariables = ShootStruct;

        //Si bug, deplacer dans une autre methode non existante
        GetComponent<StatePhaseEntity>().DamageInfo = GunData.GetWeaponDamageStats;
            
    }
    private void ExternalSetAnimator()
    {
         GetComponent<Animator>().runtimeAnimatorController = GunData.GetAnimatorGun ;
    }

    private void ExternalSetMeleeAnimationClip()
    {
        StatePhaseEntity spEntity = GetComponent<StatePhaseEntity>();

        spEntity.SetExecuteAnimation(MeleeWeaponData.GetExecuteAnimation());
        spEntity.SetAnimationForExecuted(MeleeWeaponData.GetKilledByExecutionAnimation()) ;
    }
    private void ExternalSetExecutedPosition()
    {
        StatePhaseEntity spEntity = GetComponent<StatePhaseEntity>();
        spEntity.SetExecutedLocalPosition(MeleeWeaponData.GetExecutedPosition());
    }

    private void UpdateScript() 
    {
        UpdateScriptForGun();
        UpdateScriptForMeleeWeapon();
    }
    private void UpdateScriptForGun()
    {
        if (GunData != null)
        {
            if (GunData == PreviousGunWeaponData) return;

            ExternalSetAnimator();
            ExternalSetStatePhaseEntityStructVariables(); //Si bug, supprimer cette ligne

            FirstHandGripGunGameObject.transform.localPosition = GunData.GetFirstHandGripPosition;
            SecondHandGripGunGameObject.transform.localPosition = GunData.GetSecondHandGripPosition;
            WeaponCanon.transform.localPosition = GunData.GetCanonPosition;
            WeaponCanon.transform.localRotation = Quaternion.Euler(GunData.GetCanonPosition);
        }
    }
    private void UpdateScriptForMeleeWeapon()
    {
        if(MeleeWeaponData == null) MeleeWeaponData = DefaultMeleeWeaponData ;

        ExternalSetMeleeAnimationClip();
        ExternalSetExecutedPosition();

        if(MeleeWeaponData == PreviousMeleeWeaponData) return;

        FirstHandGripMeleeGameObject.transform.localPosition = MeleeWeaponData.GetFirstHandGripPosition();
        SecondHandGripMeleeGameObject.transform.localPosition = MeleeWeaponData.GetSecondHandGripPosition();
    }

}
