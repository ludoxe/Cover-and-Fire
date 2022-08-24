using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Data_MissionObjective : ScriptableObject
{

    #region private Propreties
    [SerializeField] private bool _IsObjectiveCompleted;
    [SerializeField] private string _ObjectiveName;
    [SerializeField] private string _ObjectiveDetails;

    #endregion


    #region Public access
    public string ObjectiveName => _ObjectiveName;
    public string ObjectiveDetails => _ObjectiveDetails;

    #endregion

    #region Public access + Set
    public bool IsObjectiveCompleted {get => _IsObjectiveCompleted; set => _IsObjectiveCompleted = value; }

    #endregion

}
