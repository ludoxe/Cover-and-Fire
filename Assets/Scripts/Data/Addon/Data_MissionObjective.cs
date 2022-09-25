using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Data_MissionObjective : ScriptableObject
{
    //A instancier

    #region private Propreties
    private bool _IsObjectiveCompleted = false ;
    [SerializeField] private string _ObjectiveName;
    [SerializeField] private string _ObjectiveDetails;

    private SuperManager _SuperManager;

    #endregion

    #region Public access
    public string ObjectiveName => _ObjectiveName;
    public string ObjectiveDetails => _ObjectiveDetails;

    #endregion

    #region Public access + Set
    public bool IsObjectiveCompleted {get => _IsObjectiveCompleted; set => _IsObjectiveCompleted = value; }

    public SuperManager SuperManager { get => _SuperManager; set => _SuperManager = value; }

    #endregion

    #region Public virtual method 

    public virtual bool CheckMissionCondition() 
    {   
        //On appellera cette fonction depuis le script MissionObjectiveChecker
        // Mettre dedans la fonction qui vérifiera si _IsObjectiveCompleted = true ou false

        return false;
    } 

    //Les overloads methods
    public virtual bool CheckMissionCondition(GameObject myGameObject) => false;
    public virtual bool CheckMissionCondition(SuperManager myManager) => false;

    public virtual void InitializeObjective()
    {
        //Permet de générer dans le niveau les objectifs
    }

    #endregion



}
