using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Item/Melee Weapon")]
public class Data_Item_MeleeWeapon : Data_Item
{
    [SerializeField] private AnimationClip ExecuteAnimation ;
    [SerializeField] private AnimationClip KilledByExecutionAnimation;
    [SerializeField] private Vector2 ExecutedPosition;
    [SerializeField] private Vector2 SpriteLocalPosition;
    [SerializeField] private Vector2 FirstHandGripPosition;
    [SerializeField] private Vector2 SecondHandGripPosition;


    public AnimationClip GetExecuteAnimation() => ExecuteAnimation;
    public AnimationClip GetKilledByExecutionAnimation() => KilledByExecutionAnimation;
    public Vector2 GetExecutedPosition() => ExecutedPosition;
    public Vector2 GetSpriteLocalPosition() => SpriteLocalPosition ;
    public Vector2 GetFirstHandGripPosition() => FirstHandGripPosition;
    public Vector2 GetSecondHandGripPosition() => SecondHandGripPosition;


}
