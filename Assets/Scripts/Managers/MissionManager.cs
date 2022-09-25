using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Manager
{

    #region variables

    [SerializeField] private Data_MissionGame Mission ;


    #endregion

    #region public methods

    public void UpdateMissionStatus()
    {
        Mission.SetIsMissionCompleted();
        Mission.SetIsMissionFailed();
    }



    #endregion



}
