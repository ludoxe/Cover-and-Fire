using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CharacterAppearance : MonoBehaviour

{
    [Header("Appareance setting")]

    [Tooltip("0 = White Skin ; 1 = Black Skin")] [Range(0, 1)] [SerializeField] private int SkinColor;
    [Tooltip("0 = Female ; 1 = Male")] [Range(0, 1)] [SerializeField] private int Gender;

    [SerializeField] private Data_CustomCharacterPart_Hair HairStyle;
    [SerializeField] private Sprite EyesStyle;


    [Space(10)]

    [Header("Customizable Body Parts")]
    [SerializeField] private SpriteRenderer Head;
    [SerializeField] private SpriteRenderer Eyes;
    [SerializeField] private SpriteRenderer FrontHair;
    [SerializeField] private SpriteRenderer BackHair;
    [SerializeField] private Material BlackSkin;
    [SerializeField] private Material WhiteSkin;
    [SerializeField] private Sprite BlackHead;
    [SerializeField] private Sprite WhiteHead;
    [SerializeField] private Sprite FemaleBody;
    [SerializeField] private Sprite MaleBody;

    [SerializeField] private SpriteRenderer Body;
    [SerializeField] private List<SpriteRenderer> BodyPart;

    public struct StructAppearanceSettingsExported
    {
        public int exportedSkinColor;
        public int exportedGender;
        public Data_CustomCharacterPart_Hair exportedHairStyle;
        public Sprite exportedEyesStyle;
    }




#if UNITY_EDITOR
    private void Update()
    {
        UpdateChanges();
    }
#endif
    public StructAppearanceSettingsExported GetExportedAppearance()
    {
        var myStruct = new StructAppearanceSettingsExported();

        myStruct.exportedSkinColor = SkinColor;
        myStruct.exportedGender = Gender;
        myStruct.exportedHairStyle = HairStyle;
        myStruct.exportedEyesStyle = EyesStyle;

        return myStruct;
    }

    public void SetImportAppearanceSettings(StructAppearanceSettingsExported myStruct)
    {
        SkinColor = myStruct.exportedSkinColor;
        Gender = myStruct.exportedGender;
        HairStyle = myStruct.exportedHairStyle;
        EyesStyle = myStruct.exportedEyesStyle;
    }



    public void GetUpdateChanges()
    {
        UpdateChanges();
    }

    private void Start()
    {
        UpdateChanges();
    }



    private void UpdateChanges()
    {
        //Change Hair
        if (HairStyle.Hair["FrontHair"] != null) FrontHair.sprite = HairStyle.Hair["FrontHair"];
        else FrontHair.sprite = null;
        if (HairStyle.Hair["BackHair"] != null) BackHair.sprite = HairStyle.Hair["BackHair"];
        else BackHair.sprite = null;

        FrontHair.transform.localPosition = new Vector2(FrontHair.transform.localPosition.x, HairStyle.GetYPosition);

        //Change Eyes
        Eyes.sprite = EyesStyle;

        //Change skin     
        if (SkinColor == 0)
        {
            Head.sprite = WhiteHead;
            foreach (SpriteRenderer sprite in BodyPart) sprite.material = WhiteSkin;
        }
        if (SkinColor == 1)
        {
            Head.sprite = BlackHead;
            foreach (SpriteRenderer sprite in BodyPart) sprite.material = BlackSkin;
        }


        //Change gender
        if (Gender == 0)
        {
            Body.sprite = FemaleBody;
        }

        if (Gender == 1)
        {
            Body.sprite = MaleBody;
        }

    }




}

