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

    [SerializeField] private Color HairColor; 
    [SerializeField] private Color EyesColor; 
    [Range(0, 1)] [SerializeField] private float ShadowHairTransparency = 1;
    [Range(0, 1)] [SerializeField] private float ShadowFaceTransparency = 1;


    


    [Space(10)]


    [Header("Predefine Part Options")]
    [SerializeField] private Material BlackSkin;
    [SerializeField] private Material WhiteSkin;
    [SerializeField] private Sprite FemaleBody;
    [SerializeField] private Sprite MaleBody;

    [Space(10)]

    [Header("Body Parts Reference")]
    [SerializeField] private SpriteRenderer Head;
    [SerializeField] private SpriteRenderer Eyes;
    [SerializeField] private SpriteRenderer FrontHair;
    [SerializeField] private SpriteRenderer BackHair;
    [SerializeField] private SpriteRenderer ShadowHair;
    [SerializeField] private SpriteMask HairMask;

    [SerializeField] private SpriteRenderer ShadowFace;
    [SerializeField] private SpriteRenderer Body;
    
    [Header("All Colorable Parts")]
    [SerializeField] private List<SpriteRenderer> BodyPart;

    public struct StructAppearanceSettingsExported
    {
        public int exportedSkinColor;
        public int exportedGender;
        public Data_CustomCharacterPart_Hair exportedHairStyle;
        public Sprite exportedEyesStyle;

        public Color exportedHairColor;
        public Color exportedEyesColor;

        public float exportedShadowHairTransparency;
        public float exportedShadowFaceTransparency;
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
        myStruct.exportedHairColor = HairColor;
        myStruct.exportedEyesColor = EyesColor;
        myStruct.exportedShadowHairTransparency = ShadowHairTransparency;
        myStruct.exportedShadowFaceTransparency = ShadowFaceTransparency;

        return myStruct;
    }

    public void SetImportAppearanceSettings(StructAppearanceSettingsExported myStruct)
    {
        SkinColor = myStruct.exportedSkinColor;
        Gender = myStruct.exportedGender;
        HairStyle = myStruct.exportedHairStyle;
        EyesStyle = myStruct.exportedEyesStyle;
        HairColor = myStruct.exportedHairColor;
        EyesColor = myStruct.exportedEyesColor;
        ShadowHairTransparency = myStruct.exportedShadowHairTransparency;
        ShadowFaceTransparency = myStruct.exportedShadowFaceTransparency;
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
        FrontHair.color = HairColor;
        BackHair.color = HairColor;
        if (HairStyle.Hair["FrontHair"] != null) FrontHair.sprite = HairStyle.Hair["FrontHair"];
        else FrontHair.sprite = null;
        if (HairStyle.Hair["BackHair"] != null) BackHair.sprite = HairStyle.Hair["BackHair"];
        else BackHair.sprite = null;
        if (HairStyle.Hair["ShadowHair"] != null) ShadowHair.sprite = HairStyle.Hair["ShadowHair"];
        else ShadowHair.sprite = null;

        //Set Hair Position
        SetHairOffset(FrontHair);
        SetHairOffset(BackHair);
        SetHairOffset(ShadowHair);

        //Set Hair Mask
         if (HairStyle.Hair["FrontHair"] != null) HairMask.sprite = HairStyle.Hair["FrontHair"];
        else HairMask.sprite = null;
        HairMask.transform.position = FrontHair.transform.position ;

        //Change Eyes
        Eyes.sprite = EyesStyle;
        Eyes.color = EyesColor;

        //Change skin     
        if (SkinColor == 0)
        {
            Head.material = WhiteSkin;
            //Head.sprite = WhiteHead;
            foreach (SpriteRenderer sprite in BodyPart) sprite.material = WhiteSkin;
        }
        if (SkinColor == 1)
        {
            Head.material = BlackSkin;
            //Head.sprite = BlackHead;
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

        //Set Alpha' Shadows Part
        Color ShadowHairColor = ShadowHair.color ;
        Color ShadowFaceColor = ShadowFace.color ;

        ShadowHairColor.a = ShadowHairTransparency ;
        ShadowFaceColor.a = ShadowFaceTransparency ;

        ShadowHair.color = ShadowHairColor;
        ShadowFace.color = ShadowFaceColor;



    }

    private void SetHairOffset(SpriteRenderer Hair)
    {
        Hair.transform.localPosition = new Vector2(HairStyle.GetXPosition, HairStyle.GetYPosition);
    }




}

