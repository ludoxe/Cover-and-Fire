using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static GameObject CameraManagerGameObject ;
    public static CameraManager Manager;

    [Space(30)]

    [SerializeField] private Transform VirtualsCamerasTransform;
    [SerializeField] private CinemachineVirtualCamera PlayerCamera;
    [SerializeField] private CinemachineVirtualCamera GroupCamera;

    [Header("Cache")]
    private List<GameObject> cache_PlayerEnemiesList = new List<GameObject>() ;

    #region Receive
    private Transform ReceiveSelectedEntityAsTarget()
    {
        return TeamPlayerManager.Manager.GetSelectedSoldier().transform;
    }

    private List<GameObject> ReceiveEnemiesList(Transform Entity)
    {
        return Entity.GetComponent<StatePhaseEntity>().GetEnemiesList();
    }

    #endregion

    #region AccessComponents

    private CinemachineTargetGroup AccessTargetGroupPlayerEnemies()
    {
        return GetComponentInChildren<CinemachineTargetGroup>() ;
    }

    #endregion

    #region init Singleton
    void Awake()
    {
        if(CameraManagerGameObject != null) Destroy(this.gameObject);
        else 
        {
            CameraManagerGameObject = this.gameObject;
            DontDestroyOnLoad(CameraManagerGameObject);
            Manager = this;
        }
    }

    #endregion

    public void CenterToPlayer()
    {
        SetPlayerCameraFollowTarget(ReceiveSelectedEntityAsTarget());
        SelectActiveCamera(PlayerCamera);
    }
    public void CenterBetweenAllActors()
    {
        SelectActiveCamera(GroupCamera);
    }



    private void SelectActiveCamera(CinemachineVirtualCamera ActiveVCamera)
    {
        foreach (CinemachineVirtualCamera VCamera in VirtualsCamerasTransform.GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            VCamera.enabled = false;
        }

        ActiveVCamera.enabled = true;
    }
    private void SetPlayerCameraFollowTarget(Transform _target)
    {
        PlayerCamera.m_Follow = _target ;
    }
    
    private void UpdateGeneralTargetGroup()
    {
        List<CinemachineTargetGroup.Target> newTargetList =  new List<CinemachineTargetGroup.Target>();
        newTargetList.Add(PlayerToTargetGroup());
        newTargetList.AddRange(EnemiesListToTargetGroup());

        SetTargetGroup(newTargetList);

    }

    private void SetTargetGroup(List<CinemachineTargetGroup.Target> newCinemachineTargetList )
    {
        AccessTargetGroupPlayerEnemies().m_Targets = newCinemachineTargetList.ToArray();
    }
    private void SetTargetGroup(CinemachineTargetGroup.Target[] newCinemachineTargetList)
    {
        AccessTargetGroupPlayerEnemies().m_Targets = newCinemachineTargetList;
    }

    private List<CinemachineTargetGroup.Target> EnemiesListToTargetGroup()
    {

        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();

        //Récupérer la liste d'ennemis dans StatePhaseEntity
        foreach(GameObject Enemie in ReceiveEnemiesList(ReceiveSelectedEntityAsTarget()))
        {
            CinemachineTargetGroup.Target myTarget ;

            myTarget.target = Enemie.transform ;
            myTarget.weight = 2 ;
            myTarget.radius = 3;

            targets.Add(myTarget);

        }
        return targets ; 
    }

    private CinemachineTargetGroup.Target PlayerToTargetGroup()
    {
        CinemachineTargetGroup.Target myTarget ;
        myTarget.target = ReceiveSelectedEntityAsTarget(); ;
        myTarget.weight = 2;
        myTarget.radius = 4;

        return myTarget;
    }

    private bool EnemiesListHasChanged()
    {
        List<GameObject> EnemiesList = ReceiveEnemiesList(ReceiveSelectedEntityAsTarget());
        if (Utility.CompareContentsListsIsDifferent<GameObject>(EnemiesList, cache_PlayerEnemiesList))
        {
            cache_PlayerEnemiesList = new List<GameObject>(EnemiesList);
            return true ;
        }
        else
        {
            return false ;
        }
    }

    private void Update()
    {
        if (Time.frameCount % 25 == 0) 
        {
            if(EnemiesListHasChanged()) 
            {
                UpdateGeneralTargetGroup();
            } 
        }
    }
}
