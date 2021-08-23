using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IAControlEntity : MonoBehaviour
{
    public EnumState State;
    private GameObject Entity;

    private void Start()
    {
        Entity = this.gameObject;
       
    }

    private void OnValidate()
    {
        if(Entity != null)
        Entity.GetComponent<StatePhaseEntity>().SetState(State);
    }
    private void Update()
    {

        State = GetComponent<StatePhaseEntity>().GetState;

    }

}
