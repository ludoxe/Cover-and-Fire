using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Singleton")]

    public static GameObject GameObjectManager;
    public static GameManager Manager;

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
        List<GameObject> newList = new List<GameObject>(Team1);
        return newList;
    }
    public List<GameObject> GetTeam2Entities()
    {
        List<GameObject> newList = new List<GameObject>(Team2);
        return newList;
    }
    public List<GameObject> GetCovers()
    {
        List<GameObject> newList = new List<GameObject>(Covers);
        return newList;
    }

    #endregion

    #region Singleton Init

    private void Awake()
    {
        if(!GameObjectManager)
        {
            GameObjectManager = this.gameObject;
            Manager = this ;
        }
        else if (GameObjectManager)
        {
            Destroy(this.gameObject);
        }

        SearchAllEntitesInGame();

    }
    #endregion

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





}
