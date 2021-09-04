using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cover : MonoBehaviour
{
    [Header("Cover Infos")]
    [SerializeField] private float Health;
    [SerializeField] private int Capacity;
    [SerializeField] private bool IsCoverTouchable = true;
    [Range(-1,1)][SerializeField] private int DirectionToUse = 1;
    [SerializeField] private List<Transform> CoverBounds = new List<Transform>(2) { null,null};


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


    #region ExternalSet
    private void ExternalSetAllEntityStruct(GameObject Entity)
    {
        ExternalSetStatePhaseEntityCoverAnimation(Entity);
        ExternalSetStatePhaseEntityCoverCharacterPredifineTransform(Entity);
    }
    private void ExternalSetStatePhaseEntityCoverAnimation(GameObject Entity)
    {
        //On cr�er newStruct
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
        //On cr�er newStruct
        StatePhaseEntity.StructCharacterTransform newStruct = new StatePhaseEntity.StructCharacterTransform();

        //On assigne les valeurs de newStruct
        newStruct.PositionInCover = transform.TransformPoint(LocalPositionInCover);
        newStruct.IsFacingEnnemyInCover = IsFacingEnnemyInCover;

        //On remplace les structs de l'Entity par newStruct
        Entity.GetComponent<StatePhaseEntity>().CoverCharacterPredifineTransform = newStruct;
    }
    private void ExternalSetStatePhaseEntityCover(GameObject Entity, GameObject Cover)
    {
        Entity.GetComponent<StatePhaseEntity>().SetCover(Cover);
    }

    private void ExternalSetStatePhaseEntityIsCoverTouchable(GameObject Entity, bool IsCoverTouchable)
    {
        Entity.GetComponent<StatePhaseEntity>().SetIsInCoverTouchable(IsCoverTouchable);
    }

    #endregion

    #region Public Get

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.isTrigger) return;

           

            if (EntityUsingCover.Count < Capacity)
            {
                //D�finir l'entit�
                var Player = collision.gameObject;

                //Ajouter l'entit� dans la liste
                EntityUsingCover.Add(Player);

                //D�finir les structs de l'entit�
                ExternalSetAllEntityStruct(Player);

                //Activer le state EnterInCover de l'entit�
                Player.GetComponent<StatePhaseEntity>().SetState(EnumState.EnterInCover) ;

                //D�finis la couverture actuelle du joueur
                ExternalSetStatePhaseEntityCover(Player, this.gameObject);

                //D�finis la couverture comme touchable ou non
                ExternalSetStatePhaseEntityIsCoverTouchable(Player, IsCoverTouchable);

                //D�finir les zones expos�es de la couverture
                Player.GetComponent<BoundTarget>().SetExposedPartOfEntity(!ProtectHead, !ProtectBody);

                //D�finis la zone de tir de la couverture
                Player.GetComponent<BoundTarget>().SetCoverBounds(CoverBounds);

            }

        }
    }
    //Appel� quand l'entit� quitte la couverture ou quand cette derni�re est d�truire
    private void ExitCover()
    {
        foreach (GameObject Entity in EntityUsingCover)
        {
            //D�sactiver le state EnterInCover de l'entit�

            //D�sactiver la couverture actuelle du joueur
            ExternalSetStatePhaseEntityCover(Entity, null);

            //D�finir les zones expos�es de la couverture
            Entity.GetComponent<BoundTarget>().SetExposedPartOfEntity(true, true);

            //D�finis la zone de tir de la couverture
            Entity.GetComponent<BoundTarget>().SetCoverBounds(null);
        }
    }
}
