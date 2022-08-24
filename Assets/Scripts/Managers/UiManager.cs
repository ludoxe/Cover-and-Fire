using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class UiManager : Manager
{

    [Header("Singleton")]
    public static GameObject UiManagerGameObject ;
    public static UiManager Manager ;

    [Header("UI Menu ")]
    private GameObject TitlePanel ;
    private GameObject DescriptionPanel;
    private GameObject ImagePanel;
    private GameObject OptionPanelGroup;

    [Header("UI Components ")]
    private TextMeshProUGUI Title ;
    private TextMeshProUGUI Description;
    private Image _Image ;

    [Header("Cache")]

    private GameObject OptionPanelCache;
    

    #region Singleton


    private void Awake()
    {
        if(UiManagerGameObject == null) 
        {

            UiManagerGameObject = this.gameObject ;
            Manager = this ;
        
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    public void ShowMenu(
        GameObject _UI,
        string _Title = null,
        string _Description = null,
        Sprite _Image = null,
        Action[] _Options = null)
    {
        if (_UI == null)
        {
            Debug.Log("UI == null");
            return;
        }
        GetReference(_UI);

        if(_Title == null) TitlePanel.SetActive(false);
        else 
        {
            Title.text = _Title ;
        }
        if (_Description == null) DescriptionPanel.SetActive(false);
        else
        {
            Description.text = _Description;
        }
        if (_Image == null) ImagePanel.SetActive(false);
        else
        {
            this._Image.sprite = _Image;
        }
        if (_Options == null) OptionPanelGroup.SetActive(false);
        else
        {
            foreach(Action myAction in _Options) 
            {
                var newOption = Instantiate(OptionPanelCache, OptionPanelGroup.transform);
            }
            

        }
        
        


    }

    private void GetReference(GameObject UI)
    {
        if (UI.TryGetComponent(out Window _Window)) {
            // Mise à jour des panels
            if (_Window.TitlePanel)
                TitlePanel = _Window.TitlePanel;
            if (_Window.DescriptionPanel)
                DescriptionPanel = _Window.DescriptionPanel;
            if (_Window.ImagePanel)
                ImagePanel = _Window.ImagePanel;
            if (_Window.OptionPanelGroup)
                OptionPanelGroup = _Window.OptionPanelGroup;
            if (_Window.OptionPanelCache)
                OptionPanelCache = _Window.OptionPanelCache;

            //Mise à jour des composants
            if(TitlePanel && TitlePanel.GetComponentInChildren<TextMeshProUGUI>())
            Title = TitlePanel.GetComponentInChildren<TextMeshProUGUI>();

            if (DescriptionPanel && DescriptionPanel.GetComponentInChildren<TextMeshProUGUI>())
                Description = DescriptionPanel.GetComponentInChildren<TextMeshProUGUI>();

            if (ImagePanel && ImagePanel.GetComponentInChildren<Image>())
                _Image = ImagePanel.GetComponentInChildren<Image>();

        }

    }


}
