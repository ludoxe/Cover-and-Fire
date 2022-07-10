using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;

public class Interface_Action : MonoBehaviour
{
    

    private EnumState State = EnumState.InCover;
    [SerializeField]private GameObject PlayerEntity;


    public void SetState(EnumState state)
    {
        State = state;
    }

    private void Update()
    {
        if (!PlayerEntity) return;
        StateSelection();
    }

    private void StateSelection()
    {
        var NewState = PlayerEntity.GetComponent<StatePhaseEntity>().GetState;
        State = NewState;

        switch (State)
        {

            case EnumState.Running:
            return;
            
            case EnumState.EnterInCover:        
            return;

            case EnumState.InCover:        
            if (TouchControl.ControlLockerActions == false)
            {
                if (TouchControl.SwipeUp) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.InWaitingPosition);
            }
                return;

            case EnumState.InWaitingPosition:
        
            if (TouchControl.ControlLockerActions == false)
            {
                if (TouchControl.SwipeDown) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.InCover);
                else if (TouchControl.SwipeRight) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.ExitCover);
            }
                return;

            case EnumState.InAimPosition:
        
            if (TouchControl.ControlLockerActions == false)
            {
                    if (TouchControl.SwipeDown) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.InCover);
                    else if (TouchControl.FingerHold) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.InFire);
                    else if (TouchControl.SwipeRight) PlayerEntity.GetComponent<StatePhaseEntity>().SetEnemySelectedByIncrement(+1);
                    else if (TouchControl.SwipeLeft) PlayerEntity.GetComponent<StatePhaseEntity>().SetEnemySelectedByIncrement(-1);
                }
                return;

            case EnumState.InFire:
        
            if (TouchControl.ControlLockerActions == false)
            {
                if (TouchControl.FingerHold == false) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.InWaitingPosition);
                if (TouchControl.FingerHold == true) PlayerEntity.GetComponent<StatePhaseEntity>().SetState(EnumState.InFire);
            }
                return;

            case EnumState.ExitCover:
            return;

            default:
                Debug.LogError(State + " isn't valid");
                return;
        

        }
    }



}
