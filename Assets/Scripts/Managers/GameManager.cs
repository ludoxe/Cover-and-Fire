using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    
    /* 
    Le gameManager détermine si le joueur a gagné ou perdu en:
    - Contrôlant l'Etat de la partie (joueurs et ennemis présents)
    - Contrôlant l'Etat des missions 
    - Gérant la situation de win ou de lose
    */


    [Header("Parameters")]
    [SerializeField] private Team TeamLayerPlayer = Team.Team1;

    private List<GameObject> Team1 = new List<GameObject>();
    private List<GameObject> Team2 = new List<GameObject>();
    private List<GameObject> Covers = new List<GameObject>();

    [Header("Data")]
    [SerializeField] private Data_UiWindow GameOverMenu;

    [Header("Dependencies")]
    private MissionManager _MissionManager;

    #region Public Access

    public List<GameObject> GetAllEntities()
    {
        List<GameObject> newList = new List<GameObject>(Team1);
        newList.AddRange(Team2);
        return newList;
    }
    public List<GameObject> GetTeamEntities(Team TeamLayer)
    {

        

        if(TeamLayer == Team.Team1) return Team1;
        else if (TeamLayer == Team.Team2) return Team2;
        else 
        {
            Debug.Log("error");
            return null;
        }
    }
    public List<GameObject> GetTeam1Entities()
    {
        return GetTeamEntities(Team.Team1);
    }
    public List<GameObject> GetTeam2Entities()
    {
        return GetTeamEntities(Team.Team2);
    }
    public List<GameObject> GetTeamPlayerEntities()
    {
        if (TeamLayerPlayer == Team.Team1) return GetTeam1Entities();
        else return GetTeam2Entities();
    }
    public List<GameObject> GetTeamEnemyEntities()
    {
        if (TeamLayerPlayer == Team.Team1) return GetTeam2Entities();
        else return GetTeam1Entities();
    }
    public Team GetTeamLayerPlayer()
    {
        return TeamLayerPlayer;
    }
    public Team GetTeamLayerEnemy()
    {
        return TeamLayerPlayer;
    }
    public List<GameObject> GetCovers()
    {
        List<GameObject> newList = new List<GameObject>(Covers);
        return newList;
    }

    #endregion

    #region Public Methods
    public void UpdateEntitiesLists()
    {
        SearchAllEntitesInGame();
        if(GetTeamPlayerEntities().Count == 0) EndGame();
    }
    #endregion

    private void Start() 
    {

        _MissionManager = SuperManager.GetSuperManager().GetManager<MissionManager>() as MissionManager ;

        SearchAllEntitesInGame(); //A supprimer quand on chargera les niveaux
    }



    private void SearchAllEntitesInGame() // A remplacer plus tard par un système manuel lorsque l'on chargera le niveau
    {
        List<GameObject> myTeam1List = new List<GameObject>();
        List<GameObject> myTeam2List = new List<GameObject>();

        foreach(GameObject Entity in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(LayerMask.LayerToName(Entity.layer) == "Team1") myTeam1List.Add(Entity);
            if (LayerMask.LayerToName(Entity.layer) == "Team2") myTeam2List.Add(Entity);
        }

        Team1 = myTeam1List;
        Team2 = myTeam2List;
    }

    private bool IsTeamDestroyed(Team DestroyedTeam)
    {
        if(GetTeamEntities(DestroyedTeam).Count == 0) return true;
        else return false;
    }

    private void EndGame()
    {
        if(IsTeamDestroyed(TeamLayerPlayer))
        {
            GameOverMenu.ShowMenu();
        }
    }





}
