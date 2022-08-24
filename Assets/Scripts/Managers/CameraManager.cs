using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

using Cinemachine;

public class CameraManager : Manager
{
    [Header("Dependencies")]
    [SerializeField] private TeamPlayerManager TeamPlayerManager;



    [SerializeField] private Transform VirtualsCamerasTransform;
    [SerializeField] private CinemachineVirtualCamera PlayerCamera;
    [SerializeField] private CinemachineVirtualCamera GroupCamera;

    [Header("Cache")]
    private List<GameObject> cache_PlayerEnemiesList = new List<GameObject>() ;

    #region Receive
    private Transform ReceiveSelectedEntityAsTarget()
    {
        return TeamPlayerManager.GetSelectedSoldier()?.transform;
    }

    private List<GameObject> ReceiveEnemiesList(Transform Entity)
    {
        return Entity.GetComponent<StatePhaseEntity>().GetEnemiesList();
    }

    #endregion

    #region Public CameraFocus
    public void CenterToPlayer()
    {
        SetPlayerCameraFollowTarget(ReceiveSelectedEntityAsTarget());
        SelectActiveCamera(PlayerCamera);
    }
    public void CenterBetweenAllActors()
    {
        SelectActiveCamera(GroupCamera);
    }
    #endregion

    #region AccessComponents

    private CinemachineTargetGroup AccessTargetGroupPlayerEnemies()
    {
        return GetComponentInChildren<CinemachineTargetGroup>() ;
    }

    #endregion

    #region private Set
    private void SetTargetGroup(List<CinemachineTargetGroup.Target> newCinemachineTargetList)
    {
        AccessTargetGroupPlayerEnemies().m_Targets = newCinemachineTargetList.ToArray();
    }
    private void SetTargetGroup(CinemachineTargetGroup.Target[] newCinemachineTargetList)
    {
        AccessTargetGroupPlayerEnemies().m_Targets = newCinemachineTargetList;
    }
    private void SetPlayerCameraFollowTarget(Transform _target)
    {
        PlayerCamera.m_Follow = _target;
    }

    #endregion

    #region init

    private void Start()
    {
        if(TeamPlayerManager == null)
        {
            SuperManager _SuperManager = GameObject.Find("Super Manager").GetComponent<SuperManager>();
            TeamPlayerManager = _SuperManager.GetManager<TeamPlayerManager>() as TeamPlayerManager;
        }
    }

    #endregion

    #region Update
    private void Update()
    {
        if (Time.frameCount % 25 == 0)
        {
            if (PlayerNoLongerExist())
            {
                //Destroy(this.gameObject);
                return;
            }
            if (EnemiesListHasChanged())
            {
                UpdateGeneralTargetGroup();
            }
        }
    }
    private void UpdateGeneralTargetGroup()
    {
        List<CinemachineTargetGroup.Target> newTargetList = new List<CinemachineTargetGroup.Target>();
        newTargetList.Add(PlayerToTargetGroup());
        newTargetList.AddRange(EnemiesListToTargetGroup());

        SetTargetGroup(newTargetList);
    }
    #endregion

    #region bool Check
    private bool EnemiesListHasChanged()
    {
        List<GameObject> EnemiesList = ReceiveEnemiesList(ReceiveSelectedEntityAsTarget());
        if (Utility.CompareContentsListsIsDifferent<GameObject>(EnemiesList, cache_PlayerEnemiesList))
        {
            cache_PlayerEnemiesList = new List<GameObject>(EnemiesList);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool PlayerNoLongerExist()
    {
        if (ReceiveSelectedEntityAsTarget() == null)
            return true;
        else return false;
    }

    #endregion

    #region Convert
    private List<CinemachineTargetGroup.Target> EnemiesListToTargetGroup()
    {

        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();

        //Récupérer la liste d'ennemis dans StatePhaseEntity
        foreach (GameObject Enemie in ReceiveEnemiesList(ReceiveSelectedEntityAsTarget()))
        {
            CinemachineTargetGroup.Target myTarget;

            myTarget.target = Enemie.transform;
            myTarget.weight = 2;
            myTarget.radius = 3;

            targets.Add(myTarget);

        }
        return targets;
    }

    private CinemachineTargetGroup.Target PlayerToTargetGroup()
    {
        CinemachineTargetGroup.Target myTarget;
        myTarget.target = ReceiveSelectedEntityAsTarget(); ;
        myTarget.weight = 2;
        myTarget.radius = 4;

        return myTarget;
    }

    #endregion

    #region Select
    private void SelectActiveCamera(CinemachineVirtualCamera ActiveVCamera)
    {
        foreach (CinemachineVirtualCamera VCamera in VirtualsCamerasTransform.GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            VCamera.enabled = false;
        }

        ActiveVCamera.enabled = true;
    }

    #endregion


}
