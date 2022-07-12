using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static GameObject CameraManagerGameObject ;
    public static CameraManager Singleton;

    [SerializeField] private Transform VirtualsCamerasTransform;
    [SerializeField] private CinemachineVirtualCamera PlayerCamera;
    [SerializeField] private CinemachineVirtualCamera GroupCamera;



    #region init
    void Awake()
    {
        if(CameraManagerGameObject != null) Destroy(this.gameObject);
        else 
        {
            CameraManagerGameObject = this.gameObject;
            DontDestroyOnLoad(CameraManagerGameObject);
        }
    }

    void Start()
    {
        Singleton = this ;     
    }

    #endregion

    public void CenterToPlayer()
    {
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
}
