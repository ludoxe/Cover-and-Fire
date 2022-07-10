using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private static GameObject CameraManagerGameObject ;

    void Awake()
    {
        if(CameraManagerGameObject != null) Destroy(this.gameObject);
        else 
        {
            CameraManagerGameObject = this.gameObject;
            DontDestroyOnLoad(CameraManagerGameObject);
        }
    }
}
