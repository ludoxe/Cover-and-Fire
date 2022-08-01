using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "DataDriven/Action/ChangeScene", fileName = "Scene Changer")]
public class Data_Action_ChangeScene : Data_Action
{

    public string sceneName ;
    internal override void Action()
    {
        SceneManager.LoadScene(sceneName);
    }

}
