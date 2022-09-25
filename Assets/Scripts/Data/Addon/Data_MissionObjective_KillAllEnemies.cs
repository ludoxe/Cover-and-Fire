using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataDriven/Game/Objective/Kill all enemies", fileName = "Kill all")]
public class Data_MissionObjective_KillAllEnemies : Data_MissionObjective
{

    private GameManager _GameManager;

    #region access and Set Propreties

    private GameManager GameManager {get => _GameManager; set => _GameManager = value;  } 

    #endregion

    #region Public Access And Set Methods


    public void SetGameManager()
    {
        GameManager = SuperManager.GetManager<GameManager>() as GameManager;
    }
    public GameManager GetGameManager()
    {
        return GameManager;
    }

    #endregion

    public override bool CheckMissionCondition(SuperManager ManagerTest)
    {
        if(IsObjectiveCompleted) return true;

        if(GameManager.GetTeamEnemyEntities().Count <= 0)
        {
                return true;
        }
        else
        {
                return false;
        } 
    }

    public override void InitializeObjective()
    {
        SetGameManager();
    }
}