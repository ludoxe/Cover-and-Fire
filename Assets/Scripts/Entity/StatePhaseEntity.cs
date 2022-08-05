using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.IK;
using System.Linq;

public enum EnumState
{
    Running,
    EnterInCover,
    InCover,
    InWaitingPosition,
    InAimPosition,
    InFire,
    ExitCover,
    MeleeExecute,
    MeleeExecuted
}

public class StatePhaseEntity : MonoBehaviour
{
    /* Pour rajouter une nouvelle animation, il faut :
     * L'ajouter dans StructAnimationWithCover
     * L'ajouter dans SelectAnimatorTrueBool()
     * L'ajouter dans ChangeState()
     * 
     * Dans cover.cs
     * L'ajouter dans les variables serialized
     * L'ajouter dans ExternalSetStatePhaseEntityCoverAnimation
     * 
     * Dans l'animator
     * Rajouter le state
     * Rajouter la condition
     * 
     * Dans Interface_Action.cs
     * Verifier
     */

    [Header("Id")]
    [SerializeField] private bool IsPlayer = false;

    [Header("State")]
    private EnumState State = EnumState.Running;
    private EnumState previousState;
    [SerializeField] [Range(-1, 1)] private int WalkingDirection = 1;
    [SerializeField] private float SpeedMovement = 2 ;


    [Header("Cover Info")]
    private bool IsCovered = false;
    private GameObject Cover;
    private bool IsInCoverTouchable;

    private Animator animator;

    [Header("structs")]
    public Utility.StructAnimationWithCover CoverAnimation = new Utility.StructAnimationWithCover();
    public Utility.StructCharacterTransform CoverCharacterPredifineTransform = new Utility.StructCharacterTransform();
    public Utility.StructAimVariables AimVariables = new Utility.StructAimVariables();
    public Utility.StructShootVariables ShootVariables = new Utility.StructShootVariables();
    public Utility.StructDamageInfo DamageInfo = new Utility.StructDamageInfo();

    [Header("Entity Detector")]
    [SerializeField] private List<GameObject> EnemiesList = new List<GameObject>();
    [SerializeField] private int EnemySelector = -1;
    private GameObject Target;

    [Header("Target Bound")]
    [SerializeField] private List<Transform> CacheTargetBoundsListUncovered;
    [SerializeField] private List<Transform> CacheCoverBoundsList;

    [Header("Data Driven")]
    [SerializeField] private Data_UiWindow Menu ;
    

    private List<Transform> TargetBoundsListForAim //
    {
        get
        {
            bool TargetIsCovered;
            if (Target.TryGetComponent(out StatePhaseEntity EntityScript))
            {
                TargetIsCovered = EntityScript.GetIsCovered();
            }
            else
            {
                return CacheTargetBoundsListUncovered;
            }


            if (TargetIsCovered) return CacheCoverBoundsList;
            else return CacheTargetBoundsListUncovered;
        }
    }
    private Dictionary<string, bool> TargetExposeInfo
    {
        get
        {
            if (Target == null) return null;
            return Target.GetComponent<BoundTarget>().GetExposedParts();
        }
    }

    [Range(0, 1)] private float RandomAim;
    private int EnemiesLayer
    {
        get
        {
            int result = -1;

            //D�finir le layer des ennemis
            if (gameObject.layer == LayerMask.NameToLayer("Team1")) result = LayerMask.NameToLayer("Team2");
            if (gameObject.layer == LayerMask.NameToLayer("Team2")) result = LayerMask.NameToLayer("Team1");
            return result;
        }
    }

    private List<RaycastHit2D> LinecastResult;

    #region structs

    #endregion

    #region Public Get
    public EnumState GetState { get { return State; } }

    public bool GetIsCovered()
    {
        return IsCovered;
    }

    public GameObject GetCover()
    {
        return Cover;
    }

    public bool GetIsInCoverTouchable()
    {
        return IsInCoverTouchable;
    }

    public List<Transform> GetTargetBoundsListForAim()
    {
        return TargetBoundsListForAim;
    }

    public int GetWalkingDirection()
    {
        return WalkingDirection;
    }

    public bool GetIsPlayer()
    {
        return IsPlayer;
    }

    public List<GameObject> GetEnemiesList()
    {
        return EnemiesList;
    }
    /*  pour quand on dev l'IA
    public List<GameObject> GetEnemiesList() 
    {
        return EnemiesList ;
    }
    */
    #endregion

    #region Public Set
    public void SetState(EnumState StateName)
    {
        SetPreviousState();

        State = StateName;
        ChangeState();
    }
    public void SetIsCovered(bool mybool)
    {
        IsCovered = mybool;
    }
    /// <param name="increment">set to +1 to target next enemy or -1 to target previous enemy </param>
    public void SetEnemySelectedByIncrement(int increment)
    {
        if (EnemiesList.Contains(Target)) SetEnemySeletor(EnemiesList.IndexOf(Target) + increment);//Si dans la liste, l'ennemi selectionn� est le m�me, on rajoute ensuite le nombre d�sir� par les param�tres
        UpdateSelectedEnemy();
    }
    public void SetEnemySelectedByGameObject(GameObject gameObject)
    {
        if (EnemiesList.Contains(gameObject)) SetEnemySeletor(EnemiesList.IndexOf(gameObject));
        UpdateSelectedEnemy();
    }
    public void SetEnemySeletor(int select = 0)
    {
        if (Target != null)
        {
            EnemySelector = select;
        }
        else
            EnemySelector = 0;
    }
    public void SetCover(GameObject Cover)
    {
        this.Cover = Cover;
    }
    public void SetIsInCoverTouchable(bool IsTouchable)
    {
        IsInCoverTouchable = IsTouchable;
    }

    #endregion

    #region Public Receive
    public void ReceiveEnemiesList(List<GameObject> List)
    {
        if (List == EnemiesList) return;

        EnemiesList = new List<GameObject>(List);
        SetEnemySelectedByGameObject(Target);
    }

    #endregion

    #region AccessSingleton

    CameraManager GetCameraManager {get => CameraManager.Manager ; }


    #endregion

    #region AccessDataAddon

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Cover"), gameObject.layer);
        SetState(GetState);

    }

    //Concerne la sélection des ennemis

    private void InternalSetCacheTargetBoundsListsByAccessToGetUncoverBoundsTarget()
    {
        if (Target == null) return; //Ne fonctionne pas sans Target


        if (Target.TryGetComponent(out BoundTarget BoundTarget))
        {
            //Visera les parties expos�s de l'ennemi
            CacheTargetBoundsListUncovered = BoundTarget.GetBoundsTargetWhenUncover();
            //Visera les parties expos�s de l'ennemi si la couverture n'a pas de bounds
            if (BoundTarget.GetCoverBounds() == null) CacheCoverBoundsList = CacheTargetBoundsListUncovered;
            //Visera la couverture
            else CacheCoverBoundsList = BoundTarget.GetCoverBounds();
        }


        // S'il n'y a pas de TargetBounds, mets les TargetsBounds en null (par inf�rence, revient � d�finir la cible comme la position du Target, cf RandomTargetPositionBetweenBounds() )
        else
        {
            //Cr�er la nouvelle liste CacheTargetBoundsListCovered
            CacheCoverBoundsList = new List<Transform>(2) { null, null };

            //Cr�er la nouvelle liste CacheTargetBoundsListUncovered
            CacheTargetBoundsListUncovered = new List<Transform>(2) { null, null };
        }
    }
    private void UpdateSelectedEnemy()
    {
        CheckEnemySelector();
        SelectTargetGameObjectFromSelector();
    }
    private void CheckEnemySelector()
    {
        if (EnemySelector == -1) EnemySelector = EnemiesList.Count - 1;
        else if (EnemySelector >= EnemiesList.Count) EnemySelector = 0;
        else return;
    }
    private void SelectTargetGameObjectFromSelector()
    {
        if (EnemiesList.Count != 0)
            Target = EnemiesList[EnemySelector];

        else
            Target = null;
    }

    //Concerne l'animator
    private void SelectAnimatorTrueBool(string BoolName)
    {
        animator.SetBool("Running", false);
        animator.SetBool("EnterInCover", false);
        animator.SetBool("InCover", false);
        animator.SetBool("InWaitingPosition", false);
        animator.SetBool("InAimPosition", false);
        animator.SetBool("InFire", false);
        animator.SetBool("ExitCover", false);

        animator.SetBool(BoolName, true);
    }



    //Concerne les states de l'entity 



    private void ChangeState()
    {
        if (State == EnumState.Running) Running(); 
        else if (State == EnumState.EnterInCover) EnterInCover();
        else if (State == EnumState.InCover) InCover();
        else if (State == EnumState.InWaitingPosition) InWaitingPosition();
        else if (State == EnumState.InAimPosition) InAimPosition();
        else if (State == EnumState.InFire) InFire();
        else if (State == EnumState.ExitCover) ExitCover();
    }
    private void SetPreviousState()
    {
        previousState = State;
    }

    private void PlacePlayerToFaceWithCover()
    {
        //Mets le joueur de face dans la direction de couverture
        if (WalkingDirection == 1) transform.localEulerAngles = new Vector2(0, 0);
        if (WalkingDirection == 0) transform.localEulerAngles = new Vector2(0, 180);
    }


    #region States
    private void Running()
    {
        if (GetIsPlayer()) GetCameraManager?.CenterToPlayer();

        SetIsCovered(false);
        StartCoroutine(PlayAnimation());
        IEnumerator PlayAnimation()
        {
            //Jouer une animation
            animator.CrossFade("Running", 0.01f);
            SelectAnimatorTrueBool("Running");
            while(GetIsCovered() == false ) 
            {
                transform.Translate(new Vector3(1,0,0)*Time.deltaTime *SpeedMovement) ;
                yield return null;
            }
            yield return null;
        }
    }
    private void EnterInCover()
    {
        PlacePlayerToFaceWithCover();

        SetIsCovered(true);

        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            AnimationClip MyAnimationClip = CoverAnimation.AnimationToGetInCover;

            //Jouer l'animation pour entrer � couvert
            animator.CrossFade(MyAnimationClip.name, 0.01f);

            //S'assure que le joueur ne fait change pas d'Etat pendant l'op�ration
            TouchControl.SetLockControlActions(true);
            yield return new WaitForSeconds(MyAnimationClip.length);
            TouchControl.SetLockControlActions(false);

            //Placer l'entit� en position 
            transform.position = CoverCharacterPredifineTransform.PositionInCover;

            SetState(EnumState.InCover);
        }
    }
    private void InCover()
    {
        if(GetIsPlayer()) GetCameraManager?.CenterBetweenAllActors();

        PlacePlayerToFaceWithCover();
        SetIsCovered(true);
        //Laisse l'entit� face ou dos � l'ennemi
        if (CoverCharacterPredifineTransform.IsFacingEnnemyInCover == false)
        {
            transform.localEulerAngles = new Vector2(0, transform.localEulerAngles.y + 180);
        }

        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            //Jouer une animation
            AnimationClip MyAnimationClip = CoverAnimation.AnimationInCover;
            animator.CrossFade(MyAnimationClip.name, MyAnimationClip.length / 10);
            SelectAnimatorTrueBool("InCover");
            yield return null;
        }
    }

    private void InWaitingPosition()
    {
        PlacePlayerToFaceWithCover();
        SetIsCovered(false);
        StartCoroutine(PlayAnimation());
        IEnumerator PlayAnimation()
        {
            //Placer ceci pour fonctionner quand on se met � couvert
            AimVariables.LeftArmTarget.GetComponentInParent<LimbSolver2D>().weight = 1;

            //Jouer une animation
            SelectAnimatorTrueBool("InWaitingPosition");

            while (GetState == EnumState.InWaitingPosition)
            {
                if (EnemiesList.Count != 0)
                    SetState(EnumState.InAimPosition);
                yield return null;
            }

            yield return null;

        }

    }

    private void InAimPosition()
    {
        PlacePlayerToFaceWithCover();
        SetIsCovered(false);

        StartCoroutine(UpdateAimAngle());//Mettre l'angle de l'animation � jour
        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            //Jouer une animation 
            SelectAnimatorTrueBool("InAimPosition");

            yield return null;
        }

        IEnumerator UpdateAimAngle()
        {
            //D�finis une valeur random entre 0 et 1 qui sera appliqu� sur AimAngle()
            RandomAim = Random.value;

            while (GetState == EnumState.InAimPosition)
            {
                UpdateSelectedEnemy(); //Mettre � jour la target pour l'animation de vis�e

                InternalSetCacheTargetBoundsListsByAccessToGetUncoverBoundsTarget(); //d�finis la zone de tir possible pour toucher l'ennemi ou sa couverture

                AimAngle();
                yield return null;
            }
            yield return null;
        }
    }

    private void InFire()
    {
        if (Target == null)
        {
            SetState(EnumState.InWaitingPosition);
            return;
        }

        StartCoroutine(PlayAnimation());
        SetIsCovered(false);
        IEnumerator PlayAnimation()
        {


            //Jouer une animation
            SelectAnimatorTrueBool("InFire");

            yield return null;

            RandomAim = Random.value;
        }
    }

    private void ExitCover()
    {
        if (GetIsPlayer()) GetCameraManager?.CenterToPlayer();

        PlacePlayerToFaceWithCover();

        SetIsCovered(false);

        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            AnimationClip MyAnimationClip = CoverAnimation.AnimationToGetOutCover;

            //Jouer l'animation pour sortir de couverture
            animator.CrossFade(MyAnimationClip.name, 0.01f);

            //S'assure que le joueur ne fait change pas d'Etat pendant l'op�ration
            TouchControl.SetLockControlActions(true);
            yield return new WaitForSeconds(MyAnimationClip.length);
            TouchControl.SetLockControlActions(false);

            SetState(EnumState.Running);
        }
    }

    #endregion



    // Concerne les tirs et la vis�e 



    private void Shoot(AnimationEvent myEvent) // Cette fonction s'active dans l'event de l'animation
    {
        if (myEvent.animatorClipInfo.weight > 0.5f && Target)
        {
            LinecastResult = SendLinecastToTargetAndConvertToFilteredList(GetIsCovered());
            if (!Target) return;

            AimAngle();
            VisualBulletEffect(); //Effet de ligne atteignant sa cible
            SendDamage(); //Envoie les d�gats � ce qui est touch�


            LinecastResult = null;
        }
    }


    private Vector2 RandomTargetPositionBetweenBounds()
    {
        if (Target == null)
        {
            //Remplacer cela par un tir g�n�ral sur la couverture 

            Debug.LogError("Target is null");
            return new Vector2();
        }

        if (TargetBoundsListForAim[0] == null || TargetBoundsListForAim[1] == null)
        {
            TargetBoundsListForAim[0] = Target.transform;
            TargetBoundsListForAim[1] = Target.transform;
        }

        Vector2 VectorDirectionBetweenTargetBounds = TargetBoundsListForAim[1].position - TargetBoundsListForAim[0].position; //R�cup�re la direction entre les deux boundstarget
        Vector2 RandomTargetPositionBetweenBounds = (Vector2)TargetBoundsListForAim[0].position + RandomAim * VectorDirectionBetweenTargetBounds; //On part de LowerBounds, et on rajoute entre 0 et 1 * la direction vers Upper
        return RandomTargetPositionBetweenBounds;
    }

    private void AimAngle() //D�fini l'angle du bras pour lors du tir ou de la vis�e
    {
        if (Target == null) return; //Ne fonctionne pas sans Target

        //Syst�me de vis�e
        Transform AimReferencePoint = AimVariables.AimReferencePoint.transform;
        Vector2 TargetPosition = RandomTargetPositionBetweenBounds();
        Vector3 AngleRightDirection = Vector2.right;
        Vector3 AngleLeftDirection = Vector2.left;
        Transform WeaponCanon = ShootVariables.WeaponCanon.transform;

        //Calibration de la vis�e en fonction de l'arme
        Vector3 Calibration = WeaponCanon.position - AimReferencePoint.position;
        Vector3 TargetWithCalibration = ((Vector3)TargetPosition + Calibration * -1);

        //Calcul de l'angle de joueur vers la cible

        float Angle;
        if (transform.localEulerAngles.y == 0) //Si le joueur regarde vers la droite
            Angle = Vector2.Angle(AngleRightDirection, TargetWithCalibration - AimReferencePoint.position);
        else // Si le joueur regarde vers la gauche
            Angle = Vector2.Angle(AngleLeftDirection, TargetWithCalibration - AimReferencePoint.position);

        //D�termine Angle n�gatif si la cible est en bas 
        if (TargetWithCalibration.y < AimReferencePoint.position.y) Angle *= -1;

        //return si Angle d�passe l'intervalle de angle direction
        if (Angle < -60 || Angle > 60) return;

        //D�fini l'angle de l'animator pour correspondre � l'angle pour viser l'ennemi
        GetComponent<Animator>().SetFloat("angle direction", Angle);
    }


    // Lance une linecast de l'entit� jusqu'� la cible, retourne une Liste filtr�e
    private List<RaycastHit2D> SendLinecastToTargetAndConvertToFilteredList(bool FilterLinecastFromCover = true)
    {
        if (!Target) return null;
        Target.TryGetComponent(out StatePhaseEntity TargetIsEntity); //D�finis le script de l'Entit�
        GameObject TargetCover = null;

        if (TargetIsEntity) TargetCover = TargetIsEntity.GetCover(); //Si une entit�: d�fini Cover de Target pour l'utiliser dans Predicate();
        Transform AimReferencePoint = AimVariables.AimReferencePoint.transform;

        string ThisLayerMask = LayerMask.LayerToName(gameObject.layer);
        LayerMask MyLayerMask = LayerMask.GetMask(ThisLayerMask); //D�finis le layermask  de l'entit�


        var LinecastHitsArray = Physics2D.LinecastAll(AimReferencePoint.position, RandomTargetPositionBetweenBounds(), ~MyLayerMask); // linecast qui va du bras o� sont calcul�s les angles jusqu'� la cible
        List<RaycastHit2D> result = LinecastHitsArray.ToList(); // Convertit LinecastHitsArray en List

        result.RemoveAll(Predicate); //supprime tous les ennemis, targets, et décor qui ne sont pas la Target, si celle ci est en couverture compl�te

        return result; // on transforme la LinecastHit en List

        bool Predicate(RaycastHit2D GameObjectHit)
        {
            //Si les gameobjects hits sont des éméments de décor
            if(GameObjectHit.transform.gameObject.layer == LayerMask.NameToLayer("Decor")) return true;

            //Si les gameobjects hits sont EnemiesLayer ou TargetableLayer et en plus, si n'est pas la Target
            //On appellera tout le temps cette condition quand la cible est � d�couvert
            if ((GameObjectHit.transform.gameObject.layer == EnemiesLayer
                || GameObjectHit.transform.gameObject.layer == LayerMask.NameToLayer("Targetable"))
                && GameObjectHit.transform.gameObject != Target) return true;

            //Concerne Target si est une Entity 
            else if (TargetIsEntity)
            {

                //Si les gameObjects hits sont des covers et en plus, ne sont pas la cover de Target, et les arguments de la fonction FilterLinecastFromCover sont sur faux
                //On l'appelle quand on veut toucher la couverture de l'ennemi car la cible est � couvert
                if (GameObjectHit.transform.gameObject.layer == LayerMask.NameToLayer("Cover") && GameObjectHit.transform.gameObject != TargetCover && !FilterLinecastFromCover) return true;


                //Si les gameobjects hits sont la target, et que target est � couvert 
                //On l'appelle quand on ne veut rien toucher car la cible est � couvert
                else if (GameObjectHit.transform.gameObject == Target && TargetIsEntity.GetIsCovered())
                {
                    return true;
                }
                else return false;

            }

            //Concerne Target si est une Targetable
            //Si les gameObjects hits sont des covers, et les arguments de la fonction FilterLinecastFromCover sont sur faux
            else if (!TargetIsEntity && GameObjectHit.transform.gameObject.layer == LayerMask.NameToLayer("Cover") && !FilterLinecastFromCover)
            {
                return true;
            }

            else return false;
        }


    }
    
    private void VisualBulletEffect()
    {
        Transform WeaponCanon = ShootVariables.WeaponCanon.transform;
        GameObject BulletWeaponLineCache = ShootVariables.BulletWeaponLineCache;

        if (LinecastResult.Count > 0)
        {
            RaycastHit2D Target = LinecastResult[0];

            ProjectilesManager.Manager.ShowVisualBulletEffect(WeaponCanon.position, Target.point,BulletWeaponLineCache);
        }
        if (LinecastResult.Count == 0)
        {
            ProjectilesManager.Manager.ShowVisualBulletEffect(WeaponCanon.position, WeaponCanon.right * 500, BulletWeaponLineCache);
        }
    }
    private void SendDamage()
    {
        if (LinecastResult.Count == 0) return;
        RaycastHit2D Target = LinecastResult[0];

        if (Target.transform.TryGetComponent(out IDamageable TargetStatus))
        {
            TargetStatus.ReceiveDamage(DamageInfo);
        }

    }

}