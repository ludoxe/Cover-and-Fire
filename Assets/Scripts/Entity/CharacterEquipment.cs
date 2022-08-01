using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{

    [Header("Item")]
    [SerializeField] private Data_Item_Gun Gun;

    [Space(10)]

    [Header("Gun Renderer")]
    [SerializeField] private SpriteRenderer GunSpriteRenderer;
    [SerializeField] private GameObject FirstHandGripGameObject;
    [SerializeField] private GameObject SecondHandGripGameObject;


    [Header("Arm Target")]
    [SerializeField] private GameObject LeftArmTarget;
    [SerializeField] private GameObject RightArmTarget;

    private void OnValidate()
    {
        UpdateChanges();
    }

    private void UpdateChanges()
    {
         SetWeaponPlayer();

        GunSpriteRenderer.sprite = Gun.GetSprite;
        GunSpriteRenderer.transform.localPosition = Gun.GetGunSpriteLocalPosition;


        var TwoHandsGun = Gun.GetTwoHandsGun;

        FirstHandGripGameObject.transform.localPosition = Gun.GetFirstHandGripPosition ;
        SecondHandGripGameObject.transform.localPosition = Gun.GetSecondHandGripPosition;

        LeftArmTarget.transform.position = FirstHandGripGameObject.transform.position;
        if (TwoHandsGun)
        {
            RightArmTarget.transform.position = SecondHandGripGameObject.transform.position;
        }
    }

    private void SetWeaponPlayer()
    {
        if(GetComponent<Weapon>())
        GetComponent<Weapon>().SetWeapon = Gun ;
    }

}
