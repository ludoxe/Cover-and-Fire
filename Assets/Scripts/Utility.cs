using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    #region Methods

    public static bool CompareContentsListsIsDifferent<T>(List<T> List1, List<T> List2)
    {
        if(List1.Count != List2.Count) return true;

        for (int i = 0; i < List1.Count; i++)
        {
            if (List1[i] == null || List2[i] == null ) return true;
            
            if(!List1[i].Equals(List2[i])) return true;
        }
        return false;

    }

    public static SuperManager GetSuperManager()
    {
        return GameObject.Find("Super Manager")?.GetComponent<SuperManager>() ;
    }


    #endregion
}

#region Interfaces
public interface IDamageable
{
    void ReceiveDamage(Utility.StructDamageInfo DamageInfo);

    void SetHealth(float HealthAmount);
}

public interface ISuperInitializable
{
    void SuperInit();
}

#endregion
public enum Team
{
    Team1,
    Team2
}

