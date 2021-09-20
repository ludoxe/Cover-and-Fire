using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    #region structs
    public struct StructCharacterTransform
    {
        internal Vector2 PositionInCover;
        internal bool IsFacingEnnemyInCover;
    }
    public struct StructAnimationWithCover
    {
        internal AnimationClip AnimationToGetInCover;
        internal AnimationClip AnimationToGetOutCover;
        internal AnimationClip AnimationInCover;
        internal AnimationClip AnimationInAimPosition;
        internal AnimationClip AnimationInWaitingPosition;
        internal AnimationClip AnimationInFire;
    }
    public struct StructAimVariables
    {
        [Header("Aim Variables")]
        internal GameObject LeftArmTarget;
        internal GameObject RightArmTarget;
        internal GameObject FirstHandGripGameObject;
        internal GameObject SecondHandGripGameObject;
        internal GameObject AimReferencePoint;

    }
    public struct StructShootVariables
    {
        [Header("Shoot variables")]
        internal GameObject WeaponCanon;
        internal GameObject BulletWeaponLineCache;
    }
    public struct StructDamageInfo
    {
        internal float DamageBase;
    }

    #endregion
}



public interface IDamageable
{
    void ReceiveDamage(Utility.StructDamageInfo DamageInfo);

    void SetHealth(float HealthAmount);
}
