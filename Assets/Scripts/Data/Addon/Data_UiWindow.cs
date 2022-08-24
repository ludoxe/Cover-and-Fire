using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

[CreateAssetMenu(menuName = "DataDriven/UI/Window", fileName = "Custom Window")]
public class Data_UiWindow : ScriptableObject
{
    
    [Header("Reference")]
    [SerializeField] private GameObject DefaultUI ;

    [Header("MenuParameters")]
    [SerializeField] string TitleText;
    [SerializeField] string DescriptionText;
    [SerializeField] Sprite _ImageSprite;
    [NonReorderable][SerializeField] private ActionButton[] ActionButtons;

    #region private Get
    private GameObject GetTitlePanel(Window scriptReferences) => scriptReferences.TitlePanel;
    private GameObject GetDescriptionPanel(Window scriptReferences) => scriptReferences.DescriptionPanel;
    private GameObject GetImagePanel(Window scriptReferences) => scriptReferences.ImagePanel;
    private GameObject GetOptionPanelGroup(Window scriptReferences) => scriptReferences.OptionPanelGroup;
    private GameObject GetOptionPanelCache(Window scriptReferences) => scriptReferences.OptionPanelCache;


    private TextMeshProUGUI GetTitleText(Window Reference) => GetTitlePanel(Reference)?.GetComponentInChildren<TextMeshProUGUI>();
    private TextMeshProUGUI GetDescriptionText(Window Reference) => GetDescriptionPanel(Reference)?.GetComponentInChildren<TextMeshProUGUI>();
    private Image GetUIImage(Window Reference) => GetImagePanel(Reference)?.transform.GetChild(0)?.GetComponentInChildren<Image>();

    #endregion
    public void ShowMenu()
    {
        ShowMenu(TitleText, DescriptionText, _ImageSprite, ActionButtons);
    }
    public void ShowMenu(
        string _Title = "",
        string _Description = "",
        Sprite _Image = null,
        ActionButton[] _Options = null)
    {
        var UiInstance = Instantiate(DefaultUI);
        Window WindowScript = UiInstance.GetComponentInChildren<Window>();

        if (_Title == "") GetTitlePanel(WindowScript)?.SetActive(false);
        else
        {
            GetTitleText(WindowScript).text = _Title;
        }
        if (_Description == "") GetDescriptionPanel(WindowScript)?.SetActive(false);
        else
        {
            GetDescriptionText(WindowScript).text = _Description;
        }
        if (_Image == null) GetImagePanel(WindowScript)?.SetActive(false);
        else
        {
            GetUIImage(WindowScript).sprite = _ImageSprite;
        }
        if (_Options == null) GetOptionPanelGroup(WindowScript)?.SetActive(false);
        else
        {
            GetOptionPanelGroup(WindowScript)?.SetActive(true);

            foreach (ActionButton ActionButton in _Options)
            {
                var newOption = Instantiate(GetOptionPanelCache(WindowScript), GetOptionPanelGroup(WindowScript).transform);
                Button myButton = newOption.GetComponentInChildren<Button>();
                myButton.onClick.AddListener(() => ActionButton.Action.Action());
                myButton.GetComponentInChildren<TextMeshProUGUI>().text = ActionButton.ActionName ;
            
                //Some code here   
            }


        }

    }
    [Serializable]
    public class ActionButton 
    {
        public Data_Action Action;
        public string ActionName = "My Button";

    }
}
