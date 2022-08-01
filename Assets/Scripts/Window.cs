using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    [Header("Window Parts")]
    public GameObject TitlePanel;
    public GameObject DescriptionPanel;
    public GameObject ImagePanel;
    public GameObject OptionPanelGroup;

    [Header("Cache")]
    public GameObject OptionPanelCache;


    private void Start()
    {
        OptionPanelCache.SetActive(false);
        //OptionPanelGroup.SetActive(false);
    }
}
