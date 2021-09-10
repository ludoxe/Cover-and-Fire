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

    public static Vector3 ConvertRotationFlipToScaleFlipTransform(Transform transform)
    {
        float rotationY = transform.rotation.eulerAngles.y;
        float scaleX = 1;
        Vector3 TransformScale = transform.localScale;

        if (rotationY == 180) scaleX = -1;

        return new Vector3(TransformScale.x * scaleX, TransformScale.y, TransformScale.z);

    }

}



public interface IDamageable
{
    void ReceiveDamage(Utility.StructDamageInfo DamageInfo);
}
