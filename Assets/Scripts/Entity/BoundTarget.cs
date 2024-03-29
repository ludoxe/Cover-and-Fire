using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundTarget : MonoBehaviour
{

    [SerializeField]private Transform HeadBoundsUpperPoint ;
    [SerializeField]private Transform HeadBoundsLowerPoint;
    [SerializeField]private Transform BodyBoundsUpperPoint;
    [SerializeField]private Transform BodyBoundsLowerPoint;

    private bool IsHeadExposed;
    private bool IsBodyExposed;

    [SerializeField]private List<Transform> CoverBounds = new List<Transform>(2) { null, null };

    //Cover acc�dera � cette fonction 
    public void SetExposedPartOfEntity(bool HeadIsExposed = true, bool BodyIsExposed = true)
    {
        ExposeEntity(HeadIsExposed, BodyIsExposed);
    }

    //SetPhaseEntity acc�de � cette fonction pour tirer
    public List<Transform> GetCoverBounds() //D�fini quelle liste envoyer, la liste aura 2 transforms repr�sentant entre o� et o� on peut tirer
    {
        return CoverBounds;
    }
    public void SetCoverBounds(List<Transform> myList)
    {
        //Si la liste contient que des Bounds nulles
        if (myList[0] == null || myList[1] == null) 
        {
            CoverBounds[0] = HeadBoundsUpperPoint;
            CoverBounds[1] = BodyBoundsLowerPoint;
        }
        else
        CoverBounds = myList;
    }



    public List<Transform> GetBoundsTargetWhenUncover()
    {
        var myList = new List<Transform>(2);

        // /!\ Ne pas mettre de else if
        if (IsHeadExposed) myList.Add(HeadBoundsUpperPoint);
        if (IsBodyExposed) myList.Add(BodyBoundsLowerPoint);
        if (!IsBodyExposed) myList.Add(HeadBoundsLowerPoint);
        if (!IsHeadExposed) myList.Add(BodyBoundsUpperPoint);

        if (!IsHeadExposed && !IsBodyExposed)
        {
            myList[0] = HeadBoundsUpperPoint;
            myList[1] = BodyBoundsLowerPoint;
        }

        return myList;

    }

    public Dictionary<string, bool> GetExposedParts()
    {
        Dictionary<string,bool> myDictionary = new Dictionary<string, bool>(2) ;

        myDictionary.Add("HeadIsExposed", true);
        myDictionary.Add("BodyIsExposed", true);
        return myDictionary;

    }

    private void ExposeEntity(bool HeadIsExposed = true , bool BodyIsExposed = true) //D�fini quelle partie du corps des entit�s sont expos�s aux tirs
    {
        //Met tout false avant de d�finir ce qui sera true
        IsHeadExposed = false;
        IsBodyExposed = false;

        //Met true ce qui est en param�tre
        IsHeadExposed = HeadIsExposed;
        IsBodyExposed = BodyIsExposed;
    }



}
