using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cover : MonoBehaviour
{
    [Header("Cover Infos")]
    [SerializeField] private float Health;
    [SerializeField] private int Capacity;
    [Range(0,1)][SerializeField] private int DirectionToUse = 1;

    private List<GameObject> EntityUsingCover = new List<GameObject>();

    [Space(10)]

    [Header("Animations")]
    [SerializeField] private AnimationClip AnimationToGetInCover;
    [SerializeField] private AnimationClip AnimationToGetOutCover;
    [SerializeField] private AnimationClip AnimationInCover;
    [SerializeField] private AnimationClip AnimationInWaitingPosition;
    [SerializeField] private AnimationClip AnimationInAimPosition;
    [SerializeField] private AnimationClip AnimationInFire;


    [Space(10)]

    [Header("Character Transform")]
    [SerializeField] private Vector2 LocalPositionInCover;
    [SerializeField] private bool IsFacingEnnemyInCover;

    [Header("Cover Part When In Aim")]
    [SerializeField] private bool ProtectHead;
    [SerializeField] private bool ProtectBody;

    private void ExternalSetAllEntityStruct(GameObject Entity)
    {
        ExternalSetStatePhaseEntityCoverAnimation(Entity);
        ExternalSetStatePhaseEntityCoverCharacterPredifineTransform(Entity);
    }
    private void ExternalSetStatePhaseEntityCoverAnimation(GameObject Entity)
    {
        //On créer newStruct
        StatePhaseEntity.StructAnimationWithCover newStruct = new StatePhaseEntity.StructAnimationWithCover();

        //On assigne les valeurs de newStruct
        newStruct.AnimationToGetInCover = AnimationToGetInCover ;
        newStruct.AnimationToGetOutCover = AnimationToGetOutCover;
        newStruct.AnimationInCover = AnimationInCover;
        newStruct.AnimationInWaitingPosition = AnimationInWaitingPosition;
        newStruct.AnimationInAimPosition = AnimationInAimPosition;
        newStruct.AnimationInFire = AnimationInFire;

        //On remplace les structs de l'Entity par newStruct
        Entity.GetComponent<StatePhaseEntity>().CoverAnimation = newStruct;
    }
    private void ExternalSetStatePhaseEntityCoverCharacterPredifineTransform(GameObject Entity)
    {
        //On créer newStruct
        StatePhaseEntity.StructCharacterTransform newStruct = new StatePhaseEntity.StructCharacterTransform();

        //On assigne les valeurs de newStruct
        newStruct.PositionInCover = transform.TransformPoint(LocalPositionInCover);
        newStruct.IsFacingEnnemyInCover = IsFacingEnnemyInCover;

        //On remplace les structs de l'Entity par newStruct
        Entity.GetComponent<StatePhaseEntity>().CoverCharacterPredifineTransform = newStruct;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.isTrigger) return;

            if (EntityUsingCover.Count < Capacity)
            {
                //Définir l'entité
                var Player = collision.gameObject;

                //Ajouter l'entité dans la liste
                EntityUsingCover.Add(Player);

                //Définir les structs de l'entité
                ExternalSetAllEntityStruct(Player);

                //Activer le state EnterInCover de l'entité
                Player.GetComponent<StatePhaseEntity>().SetState(EnumState.EnterInCover) ;

                //Définir les zones exposées de la couverture
                Player.GetComponent<BoundTarget>().SetExposedPartOfEntity(!ProtectHead, !ProtectBody);

            }

        }
    }

}
