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

    //Cover accèdera à cette fonction 
    public void SetExposedPartOfEntity(bool HeadIsExposed = true, bool BodyIsExposed = true)
    {
        ExposeEntity(HeadIsExposed, BodyIsExposed);
    }

    //SetPhaseEntity accède à cette fonction pour tirer
    public List<Transform> GetBoundsTargetWhenCover() //Défini quelle liste envoyer, la liste aura 2 transforms représentant entre où et où on peut tirer
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

    public List<Transform> GetBoundsTargetWhenUncover()
    {
        var myList = new List<Transform>(2);

        myList.Add(HeadBoundsUpperPoint);
        myList.Add(BodyBoundsLowerPoint);

        return myList;

    }
    private void ExposeEntity(bool HeadIsExposed = true , bool BodyIsExposed = true) //Défini quelle partie du corps des entités sont exposés aux tirs
    {
        //Met tout false avant de définir ce qui sera true
        IsHeadExposed = false;
        IsBodyExposed = false;

        //Met true ce qui est en paramètre
        IsHeadExposed = HeadIsExposed;
        IsBodyExposed = BodyIsExposed;
    }



}
