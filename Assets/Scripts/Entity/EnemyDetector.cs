using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDetector : MonoBehaviour
{
    #region fields
    
    
    [Header("Dictionnaire des ennemis")]
    [SerializeField] private List<GameObject> EnemyInTrigger = new List<GameObject>();
    private Dictionary<GameObject, float> EnemyDetectedDictionary = new Dictionary<GameObject, float>(); // Dictionnaire des tous les ennemis et leur distance, on utilisera le dictionnaire tri�
   
    [Header("Dependances")]
    private StatePhaseEntity _StatePhaseEntity ;

    [Space(15)]


    [Header("CAC ennemis")]
    private GameObject EnemyInMeleeRange;
    [SerializeField] private float MeleeRange =  3f;


    [Space(15)]


    [Header("Triggers")]
    [SerializeField] private GameObject TriggerDetection;




    #endregion

    #region Propreties
    private int ThisLayer
    {
        get
        {
            return gameObject.layer;
        }
    }
    private int EnemiesLayer
    {
        get
        {
            if (LayerMask.LayerToName(ThisLayer) == "Team1") return LayerMask.NameToLayer("Team2");
            if (LayerMask.LayerToName(ThisLayer) == "Team2") return LayerMask.NameToLayer("Team1");
            else return LayerMask.NameToLayer("Default");
        }
    }

    #endregion

    #region Public Set
    public void SetEnemiesListToStatePhaseEntity()
    {
        //Trie par ordre croissant des Values distance, les Keys ennemies, puis converti le dico en List
        List<GameObject> MyNewList = EnemyDetectedDictionary.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
        _StatePhaseEntity.ReceiveEnemiesList(MyNewList);
    }

    #endregion

    #region Private Set

    private void ExternalSetEnemyInMeleeRange(GameObject Enemy)
    {
        _StatePhaseEntity.ReceiveEnemyInMeleeRange(Enemy) ;
    }

    #endregion

    #region init
    private void Start()
    {
        _StatePhaseEntity = GetComponent<StatePhaseEntity>();
    }
    #endregion

    #region Reset Detector

    //Utiliser cette fonction si l'entit� change de layer Team 
    private void ResetEnemyDetector()
    {
        ResetAllDictionariesAndLists();
        ResetTriggerDetection();
        SetEnemiesListToStatePhaseEntity();
    }

    private void ResetAllDictionariesAndLists()
    {
        EnemyInTrigger = new List<GameObject>();
        EnemyDetectedDictionary = new Dictionary<GameObject, float>();
    }

    private void ResetTriggerDetection()
    {
        TriggerDetection.GetComponent<Collider2D>().enabled = false;
        TriggerDetection.GetComponent<Collider2D>().enabled = true;
    }

 #endregion

#region Triggers
    private void OnTriggerEnter2D(Collider2D collision) // Fait rentrer les gameObject dans EnemyInTrigger
    {
        if (collision.isTrigger) return; // Cette fonction ne s'active pas lorsqu'on rentre en contact avec un trigger
        if (collision.attachedRigidbody == null) return;
        if (collision.attachedRigidbody.gameObject.layer != EnemiesLayer && collision.attachedRigidbody.gameObject.layer != LayerMask.NameToLayer("Targetable")) return; 
        //e
        GameObject Enemy = collision.attachedRigidbody.gameObject;
        if(!EnemyInTrigger.Contains(Enemy))
        EnemyInTrigger.Add(Enemy);

        SetEnemiesListToStatePhaseEntity();
    }

    private void OnTriggerStay2D(Collider2D collision) //Fait entrer ou sortir les gameObject de EnemyIntrigger dans EnemyDetectedDictionary
    {
        if (collision.attachedRigidbody == null) return;
        if (!EnemyInTrigger.Contains(collision.attachedRigidbody.gameObject)) return;

        GameObject Enemy = collision.attachedRigidbody.gameObject;
        var myLinecastToTarget = LinecastToTarget(Enemy);

        if (myLinecastToTarget == false) // Si l'ennemi est detect� (rien entre Joueur et ennemi)
        {
            float EnemyDistance = Vector2.Distance(this.transform.position, Enemy.transform.position);

            if (!EnemyDetectedDictionary.ContainsKey(Enemy))
            {
                EnemyDetectedDictionary.Add(Enemy, EnemyDistance);
                SetEnemiesListToStatePhaseEntity();
            }
            EnemyDetectedDictionary[Enemy] = EnemyDistance; // S'assure d'actualiser tous les r�sultats de distance

            if(GameObjectIsInRange(EnemyDistance, MeleeRange)) 
            {
                ExternalSetEnemyInMeleeRange(Enemy);
            }
            


        }
        if (myLinecastToTarget == true) // Si l'ennemi n'est pas detect� (obstacle entre Joueur et ennemi)
        {
            if (EnemyDetectedDictionary.ContainsKey(Enemy))
            {
                EnemyDetectedDictionary.Remove(Enemy);
                SetEnemiesListToStatePhaseEntity();
            }
        }

        

    }

    private void OnTriggerExit2D(Collider2D collision) // Fait sortir les gameObject de EnemyInTrigger
    {
        if (collision.isTrigger) return; // Cette fonction ne s'active pas lorsqu'on rentre en contact avec un trigger
        if (collision.attachedRigidbody == null) return;
        if (collision.attachedRigidbody.gameObject != null && collision.attachedRigidbody.gameObject.layer != EnemiesLayer && collision.attachedRigidbody.gameObject.layer != LayerMask.NameToLayer("Targetable")) return;

        GameObject Enemy = collision.attachedRigidbody.gameObject;
        RemoveEnemyFromScript(Enemy);
        SetEnemiesListToStatePhaseEntity();
    }
#endregion
    private RaycastHit2D LinecastToTarget(GameObject Target)
    {
        LayerMask MyLayerMask = LayerMask.GetMask("Cover","Team1","Team2","Targetable","Decor");
        return Physics2D.Linecast(this.transform.position, Target.transform.position, ~MyLayerMask);
    }
    private void RemoveEnemyFromScript(GameObject Enemy)
    {
        EnemyInTrigger.Remove(Enemy);
        if(EnemyDetectedDictionary.ContainsKey(Enemy)) EnemyDetectedDictionary.Remove(Enemy);
    }

    private bool GameObjectIsInRange(GameObject myGameObject, float Range)
    {
       if(Vector2.Distance(this.transform.position, myGameObject.transform.position) < Range )
       {
        return true;
       }
       else return false;
    }
    private bool GameObjectIsInRange(float CurrentDistance, float Range)
    {
        if (CurrentDistance < Range)
        {
            return true;
        }
        
        else
        {
            return false;
        }
    }

}
