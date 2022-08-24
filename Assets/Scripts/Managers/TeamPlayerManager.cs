using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPlayerManager : Manager
{

    [Header("Dependencies")]
    [SerializeField] private GameManager GameManager;

//Receive Propreties
[SerializeField] private Team TeamPlayer {get => GameManager.GetTeamLayerPlayer(); } 
private int _selectedSoldier = 0;
private int SelectedSoldier 
{
    set
    {
        if(value >= ReceiveTeamPlayer().Count)
            _selectedSoldier = 0;
        if(value < 0)
            _selectedSoldier = ReceiveTeamPlayer().Count -1;
        else
            _selectedSoldier = value;
    }
    get
    {
        return _selectedSoldier;
    }
}

#region init

private void Start()
{
    SuperManager _SuperManager = GameObject.Find("Super Manager").GetComponent<SuperManager>();
    GameManager = _SuperManager.GetManager<GameManager>() as GameManager;
}

#endregion

#region Public Get
public GameObject GetSelectedSoldier()
{
    List<GameObject> TeamPlayer = ReceiveTeamPlayer();

    if(TeamPlayer.Count > 0)    
    return TeamPlayer[SelectedSoldier];
    else return null ;
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
        return GameManager.GetTeamPlayerEntities();    
}

#endregion

#region public Set

#endregion


}
