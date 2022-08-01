using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPlayerManager : MonoBehaviour
{

public enum Team
{
 Team1,
 Team2
}

[Header("Singleton")]
public static GameObject GameObjectManager;
public static TeamPlayerManager Manager;

[Header("Data")]
[SerializeField] private Team TeamPlayer = Team.Team1 ;
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

#region Public Get
public GameObject GetSelectedSoldier()
{
    return ReceiveTeamPlayer()[SelectedSoldier];
}

#endregion

#region Public Receive

public List<GameObject> ReceiveTeam1()
{
    return GameManager.Manager.GetTeam1Entities();
}
public List<GameObject> ReceiveTeam2()
{
    return GameManager.Manager.GetTeam2Entities();
}
public List<GameObject> ReceiveTeamPlayer()
{
    if(TeamPlayer == Team.Team1) return ReceiveTeam1();
    else return ReceiveTeam2();

         
}

#endregion

#region public Set

public void SetTeamPlayer(Team newTeam)
{
    TeamPlayer = newTeam;
}

#endregion


//Initialise le singleton
private void Awake()
{
    if(GameObjectManager == null)
    {
        GameObjectManager = this.gameObject ;
        Manager = this;
        DontDestroyOnLoad(this.gameObject);
    }
    else 
    {
        Destroy(this.gameObject);
    }
}



}
