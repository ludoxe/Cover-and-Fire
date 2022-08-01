using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{

    public string KeyInput;


    [Header("Change this")]
    public Data_UiWindow Menu;

    private void Start()
    {
        
    }

private void Update()
{
    if (Input.GetKeyUp(KeyInput))
    {
            Menu.ShowMenu();
    }
}


}
