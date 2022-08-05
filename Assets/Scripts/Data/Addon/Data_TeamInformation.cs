using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataDriven/Data Addon/TeamInformation", fileName = "UniqueTeamInformation")]
public class Data_TeamInformation : ScriptableObject
{

    public enum Team
    {
        Team1,
        Team2
    }

    [Header("Dependances")]
    [SerializeField] private Data_GameInformation GameManager;

    [Header("Data")]
    private static Team TeamPlayer = Team.Team1;
    private int _selectedSoldier = 0;

    private int SelectedSoldier
    {
        set
        {
            if (value >= ReceiveTeamPlayer().Count)
                _selectedSoldier = 0;
            if (value < 0)
                _selectedSoldier = ReceiveTeamPlayer().Count - 1;
            else
                _selectedSoldier = value;
        }
        get
        {
            return _selectedSoldier;
        }
    }

    #region Public Get
    public GameObject GetSelectedSoldier()
    {
        Debug.Log("yesman");
        return ReceiveTeamPlayer()[SelectedSoldier];
    }

    #endregion

    #region Public Receive

    public List<GameObject> ReceiveTeam1()
    {
        return GameManager.GetTeam1Entities();
    }
    public List<GameObject> ReceiveTeam2()
    {
        return GameManager.GetTeam2Entities();
    }
    public List<GameObject> ReceiveTeamPlayer()
    {
        if (TeamPlayer == Team.Team1) return ReceiveTeam1();
        else return ReceiveTeam2();


    }

    #endregion

    #region public Set

    public void SetTeamPlayer(Team newTeam)
    {
        TeamPlayer = newTeam;
    }

    #endregion
}
