using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SuperManager : Manager
{
    //It manage all manager

   // private List<SuperInitializable> ObjectsToInitialize; A déplacer dans une autre classe sûrement
    private List<Manager> AllManagers;
    private List<ISuperInitializable> AllInitializableGameObject;

    #region Public access

    //Créer une fonction pour récupérer le Manager que l'on veut
    public Manager GetManager<ManagerType>()
    {
        return AllManagers.Find(x => x is ManagerType && x is Manager);
    }

    #endregion

    #region init
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        InitializeAllSuperInit();
        InitializeAllManagers();
    }
    private void InitializeAllSuperInit()
    {
        AllInitializableGameObject = GetAllSuperInitInScene();

        foreach (ISuperInitializable superInit in AllInitializableGameObject)
        {
            superInit.SuperInit();
        }
    }
    private void InitializeAllManagers()
    {
        List<GameObject> ManagersGameObject = FoundAllManagersGameObjectInScene();

        SetGameObjectsListParent(ManagersGameObject, this.transform);

        AllManagers = ConvertListGameObjectToListManagerWithoutSuperManager(ManagersGameObject);
    }
    #endregion

    #region private micro operation
    private List<Manager> ConvertListGameObjectToListManagerWithoutSuperManager(List<GameObject> myList)
    {
        List<Manager> myManagersList = new List<Manager>();

        foreach (GameObject ManagerGameObject in myList)
        {
            Manager ManagerComponent = ManagerGameObject.GetComponent<Manager>();
            if (!(ManagerComponent is SuperManager))
            {
                myManagersList.Add(ManagerComponent);
            }
        }
        return myManagersList;
    }
    private void SetGameObjectsListParent(List<GameObject> myList, Transform parent)
    {
        foreach (GameObject gameobject in myList)
        {
            gameobject.transform.SetParent(parent);
        }
    }

    private List<GameObject> FoundAllManagersGameObjectInScene()
    {
        return GameObject.FindGameObjectsWithTag("Manager").ToList();
    }

    private List<GameObject> FilterListOfSuperManager(List<GameObject> myList)
    {
        myList.RemoveAll(x => x.GetType() == typeof(SuperManager));

        return myList;
    }

    private List<ISuperInitializable> GetAllSuperInitInScene()
    {
        List<ISuperInitializable> SuperInitList ;

        SuperInitList = new List<ISuperInitializable>(FindObjectsOfType<MonoBehaviour>().OfType<ISuperInitializable>());

        return SuperInitList;
    }

#endregion

}
