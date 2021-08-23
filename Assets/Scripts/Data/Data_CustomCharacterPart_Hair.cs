using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hair", menuName = "Character/CustomCharacterPart/Hair")]
public class Data_CustomCharacterPart_Hair : CustomCharacterPart
{

    [SerializeField] private Sprite FrontHair;
    [SerializeField] private Sprite BackHair;
    [Range(-2,2)][SerializeField] private float YPosition;

    public Dictionary<string, Sprite> Hair { get { var dictionary = new Dictionary<string,Sprite>(); dictionary.Add("FrontHair",FrontHair); dictionary.Add("BackHair",BackHair); return dictionary; } }

    public float GetYPosition
    {
        get { return YPosition; }
    }

}
