using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadRagdoll : MonoBehaviour
{

    private GameManager GameManager;

    [SerializeField] private float DestroyTime = 25;

    public void SetGameManager(GameManager newGameManager) => GameManager = newGameManager;
    void Start()
    {

        GameManager?.UpdateEntitiesLists();

        StartCoroutine(DelayedDestroy());
    }


    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(DestroyTime);

        Destroy(this.gameObject);
    }
}
