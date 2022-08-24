using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hair", menuName = "Character/CustomCharacterPart/Hair")]
public class Data_CustomCharacterPart_Hair : Data_CustomCharacterPart
{

    [SerializeField] private Sprite FrontHair;
    [SerializeField] private Sprite BackHair;
    [SerializeField] private Sprite ShadowHair;
    [Range(-2,2)][SerializeField] private float YPosition;
    [Range(-2,2)][SerializeField] private float XPosition;

    public Dictionary<string, Sprite> Hair { get { var dictionary = new Dictionary<string,Sprite>(); 
    dictionary.Add("FrontHair",FrontHair); 
    dictionary.Add("BackHair",BackHair); 
    dictionary.Add("ShadowHair",ShadowHair); 
    return dictionary; } }

    public float GetYPosition
    {
        get { return YPosition; }
    }
        public float GetXPosition
    {
        get { return XPosition; }
    }

}
