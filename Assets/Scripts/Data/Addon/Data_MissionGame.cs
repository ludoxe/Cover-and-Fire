using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataDriven/Game/MissionGame", fileName = "Mission")]
public class Data_MissionGame : ScriptableObject
{

    [SerializeField] [NonReorderable] private Data_MissionObjective[] ArrayWinObjectives ;
    [SerializeField][NonReorderable] private Data_MissionObjective[] ArrayLoseObjectives;

    private bool IsMissionCompleted;
    private bool IsMissionFailed;

    public bool GetIsMissionCompleted() => IsMissionCompleted;
    public bool GetIsMissionFailed() => IsMissionFailed;
    public void SetIsMissionCompleted(bool IsCompleted) => IsMissionCompleted = IsCompleted;
    public void SetIsMissionFailed(bool IsFailed) => IsMissionFailed = IsFailed;

    private void SetIsMissionCompleted()
    {
        foreach(Data_MissionObjective Objective in ArrayWinObjectives)
        {
            if(Objective.IsObjectiveCompleted == false) 
            {
                return;
            }
        }
        SetIsMissionCompleted(true);

    }
    private void SetIsMissionFailed()
    {
        foreach (Data_MissionObjective Objective in ArrayLoseObjectives)
        {
            if (Objective.IsObjectiveCompleted == true)
            {
                SetIsMissionFailed(true);
            }
        }
        

    }




}
