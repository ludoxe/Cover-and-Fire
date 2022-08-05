using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataDriven/Data Addon/GameInformation", fileName = "UniqueGameInformation")]
public class Data_GameInformation : ScriptableObject
{
    private List<GameObject> Team1 = new List<GameObject>();
    private List<GameObject> Team2 = new List<GameObject>();
    private List<GameObject> Covers = new List<GameObject>();

    #region Public Access

    public List<GameObject> GetAllEntities()
    {
        List<GameObject> newList = new List<GameObject>(Team1);
        newList.AddRange(Team2);
        return newList;
    }
    public List<GameObject> GetTeam1Entities()
    {
        SearchAllEntitesInGame(); //A supprimer quand un système manuel chargera le niveau
        List<GameObject> newList = new List<GameObject>(Team1);
        return newList;
    }
    public List<GameObject> GetTeam2Entities()
    {
        SearchAllEntitesInGame(); //A supprimer quand un système manuel chargera le niveau
        List<GameObject> newList = new List<GameObject>(Team2);
        return newList;
    }
    public List<GameObject> GetCovers()
    {
        List<GameObject> newList = new List<GameObject>(Covers);
        return newList;
    }

    #endregion
    private void SearchAllEntitesInGame() // A remplacer plus tard par un système manuel lorsque l'on chargera le niveau
    {
        List<GameObject> myTeam1List = new List<GameObject>();
        List<GameObject> myTeam2List = new List<GameObject>();

        foreach (GameObject Entity in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (LayerMask.LayerToName(Entity.layer) == "Team1") myTeam1List.Add(Entity);
            if (LayerMask.LayerToName(Entity.layer) == "Team2") myTeam2List.Add(Entity);
        }

        Team1 = myTeam1List;
        Team2 = myTeam2List;
    }
}
