using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CharacterAppearance : MonoBehaviour

{
    [Tooltip("0 = White Skin ; 1 = Black Skin")][Range(0,1)][SerializeField] private int SkinColor;
    [Tooltip("0 = Female ; 1 = Male")] [Range(0, 1)] [SerializeField] private int Gender;

    [Space(10)]

    [Header("Swappable Body")]
    [SerializeField] private SpriteRenderer Head;
    [SerializeField] private SpriteRenderer Eyes;
    [SerializeField] private SpriteRenderer FrontHair ;
    [SerializeField] private SpriteRenderer BackHair;
    

    [SerializeField] private SpriteRenderer Body;
    [SerializeField] private List<SpriteRenderer> BodyPart;

    [Space(10)]

    [Header("Materials and Sprites")]
    [SerializeField] private Data_CustomCharacterPart_Hair HairStyle;
    [SerializeField] private Sprite EyesStyle;
    [SerializeField]private Material BlackSkin;
    [SerializeField] private Material WhiteSkin;
    [SerializeField] private Sprite BlackHead;
    [SerializeField] private Sprite WhiteHead;

    [SerializeField] private Sprite FemaleBody;
    [SerializeField] private Sprite MaleBody;

#if UNITY_EDITOR
    private void Update()
    {
        UpdateChanges();
    }
#endif
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
        Eyes.sprite = EyesStyle ;

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

