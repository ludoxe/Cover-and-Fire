using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesManager : MonoBehaviour
{

    public static GameObject ProjectilesManagerGameObject;
    public static ProjectilesManager Manager ;

    #region init Singleton
    void Awake()
    {
        if (ProjectilesManagerGameObject != null) Destroy(this.gameObject);
        else
        {
            ProjectilesManagerGameObject = this.gameObject;
            DontDestroyOnLoad(ProjectilesManagerGameObject);
            Manager = this;
        }
    }

    void Start()
    {
        ProjectilesManagerGameObject.transform.position = new Vector2();
    }

    #endregion

    public void ShowVisualBulletEffect(Vector2 Position1, Vector2 Position2, GameObject BulletLineGameObject, float spawnTime = 0.075f)
    {
        BulletLineGameObject = Instantiate(BulletLineGameObject, new Vector2(), new Quaternion());
        BulletLineGameObject.SetActive(true); // Active le gameObject pour y acc�der dans la variable ci dessous + initialiser le GO
        var BulletLine = BulletLineGameObject.GetComponent<LineRenderer>();

        //Set les points de la line renderer du canon � cible
        BulletLine.SetPosition(0, Position1);
        BulletLine.SetPosition(1, Position2);
    

        StartCoroutine(DestroyBulletLine());

        IEnumerator DestroyBulletLine()
        {
            yield return new WaitForSeconds(0.075f);
            Destroy(BulletLineGameObject);
        }
    }
    
}
