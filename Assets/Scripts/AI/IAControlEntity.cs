using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IAControlEntity : MonoBehaviour
{
    public EnumState State;
    private GameObject Entity;

    public void ExternalSetEntityState(EnumState newState)
    {
        Entity.GetComponent<StatePhaseEntity>().SetState(newState);
    }

    public EnumState ReceiveEntityState()
    {
        return GetComponent<StatePhaseEntity>().GetState;
    }
    public GameObject ReceiveCover()
    {
        return GetComponent<StatePhaseEntity>().GetCover();
    }
    private void Start()
    {
        Entity = this.gameObject; 

        StartCoroutine(RandomAI());      
    }

    private void OnValidate()
    {
        if(Entity != null)
        ExternalSetEntityState(State);
    }
    private void Update()
    {
        State = ReceiveEntityState();
    }

    private IEnumerator RandomAI()
    {
        IEnumerator FireCoroutineVar = null ;

        yield return null;
        while(true)
        {
            if(State == EnumState.InCover) 
            {
                yield return new WaitForSeconds(Random.Range(2, 6));
                ExternalSetEntityState(EnumState.InWaitingPosition);
            }
            else if(State == EnumState.InWaitingPosition) 
            {
                yield return new WaitForSeconds(Random.Range(0, 5));
                ExternalSetEntityState(EnumState.InCover);
            }
            else if(State == EnumState.InAimPosition)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 1));
                ExternalSetEntityState(EnumState.InFire);
            }
            else if(State == EnumState.InFire)
            {
                if(FireCoroutineVar == null)
                    FireCoroutineVar = FireCoroutine();
 
                StartCoroutine(FireCoroutineVar);

                yield return new WaitForSeconds(Random.Range(0.2f, 4));
                if(ReceiveCover())
                {
                    ExternalSetEntityState(EnumState.InCover);
                    StopCoroutine(FireCoroutineVar);

                }
                else 
                {
                    ExternalSetEntityState(EnumState.ExitCover);
                    StopCoroutine(FireCoroutineVar);

                }
            }
            yield return null;
        }

        IEnumerator FireCoroutine()
        {
            yield return null;
            while(true)
            {
                ExternalSetEntityState(EnumState.InFire);
                yield return null;
            }
        }
    }

}
