using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Item : ScriptableObject
{
    [SerializeField] private string Name;
    [SerializeField] private string Id;
    [SerializeField] private Sprite Logo;
    [SerializeField] private Sprite sprite;


    public Sprite GetLogo { get { return Logo; } }
    public Sprite GetSprite { get { return sprite; } }
}
