using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.IK;
using System.Linq;

public enum EnumState
{
    EnterInCover,
    InCover,
    InWaitingPosition,
    InAimPosition,
    InFire
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

    [Header("State")]
    private EnumState State = EnumState.InCover;
    private EnumState previousState;
    [SerializeField] [Range(0, 1)] private int WalkingDirection = 1;

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

            //Définir le layer des ennemis
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
        if (EnemiesList.Contains(Target)) SetEnemySeletor(EnemiesList.IndexOf(Target) + increment);//Si dans la liste, l'ennemi selectionné est le même, on rajoute ensuite le nombre désiré par les paramètres
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

    private void Start()
    {
        animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Cover"), gameObject.layer);

    }

    private void InternalSetCacheTargetBoundsListsByAccessToGetUncoverBoundsTarget()
    {
        if (Target == null) return; //Ne fonctionne pas sans Target


        if (Target.TryGetComponent(out BoundTarget BoundTarget))
        {
            //Visera les parties exposés de l'ennemi
            CacheTargetBoundsListUncovered = BoundTarget.GetBoundsTargetWhenUncover();
            //Visera les parties exposés de l'ennemi si la couverture n'a pas de bounds
            if (BoundTarget.GetCoverBounds() == null) CacheCoverBoundsList = CacheTargetBoundsListUncovered;
            //Visera la couverture
            else CacheCoverBoundsList = BoundTarget.GetCoverBounds();
        }


        // S'il n'y a pas de TargetBounds, mets les TargetsBounds en null (par inférence, revient à définir la cible comme la position du Target, cf RandomTargetPositionBetweenBounds() )
        else
        {
            //Créer la nouvelle liste CacheTargetBoundsListCovered
            CacheCoverBoundsList = new List<Transform>(2) { null, null };

            //Créer la nouvelle liste CacheTargetBoundsListUncovered
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
    private void SelectAnimatorTrueBool(string BoolName)
    {
        animator.SetBool("EnterInCover", false);
        animator.SetBool("InCover", false);
        animator.SetBool("InWaitingPosition", false);
        animator.SetBool("InAimPosition", false);
        animator.SetBool("InFire", false);

        animator.SetBool(BoolName, true);
    }



    //Concerne les states de l'entity 



    private void ChangeState()
    {
        if (State == EnumState.EnterInCover) EnterInCover();
        else if (State == EnumState.InCover) InCover();
        else if (State == EnumState.InWaitingPosition) InWaitingPosition();
        else if (State == EnumState.InAimPosition) InAimPosition();
        else if (State == EnumState.InFire) InFire();

        else Debug.LogError("State Name Invalid");
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

    private void EnterInCover()
    {
        PlacePlayerToFaceWithCover();

        SetIsCovered(true);

        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            print("EnterInCover");
            AnimationClip MyAnimationClip = CoverAnimation.AnimationToGetInCover;

            //Jouer l'animation pour entrer à couvert
            animator.CrossFade(MyAnimationClip.name, 0.3f);

            //S'assure que le joueur ne fait change pas d'Etat pendant l'opération
            TouchControl.SetLockControlActions(true);
            yield return new WaitForSeconds(MyAnimationClip.length);
            TouchControl.SetLockControlActions(false);

            //Placer l'entité en position 
            transform.position = CoverCharacterPredifineTransform.PositionInCover;

            SetState(EnumState.InCover);
        }
    }
    private void InCover()
    {
        PlacePlayerToFaceWithCover();
        SetIsCovered(true);
        //Laisse l'entité face ou dos à l'ennemi
        if (CoverCharacterPredifineTransform.IsFacingEnnemyInCover == false)
        {
            transform.localEulerAngles = new Vector2(0, transform.localEulerAngles.y + 180);
        }

        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            print("InCover");

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
            print("InWaitingPosition");

            if (previousState != EnumState.InFire)
            {
                //Placer ceci pour fonctionner quand on se met à couvert
                AimVariables.LeftArmTarget.GetComponentInParent<LimbSolver2D>().weight = 1;
                //Jouer une animation
                AnimationClip MyAnimationClip = CoverAnimation.AnimationInWaitingPosition;
                SelectAnimatorTrueBool(MyAnimationClip.name);

            }

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

        StartCoroutine(UpdateAimAngle());//Mettre l'angle de l'animation à jour
        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            print("InAimPosition");

            //Jouer une animation 
            AnimationClip MyAnimationClip = CoverAnimation.AnimationInAimPosition;
            SelectAnimatorTrueBool("InAimPosition");

            yield return new WaitForSeconds(MyAnimationClip.length);

            yield return null;
        }

        IEnumerator UpdateAimAngle()
        {
            //Définis une valeur random entre 0 et 1 qui sera appliqué sur AimAngle()
            RandomAim = Random.value;

            while (GetState == EnumState.InAimPosition)
            {
                UpdateSelectedEnemy(); //Mettre à jour la target pour l'animation de visée

                InternalSetCacheTargetBoundsListsByAccessToGetUncoverBoundsTarget(); //définis la zone de tir possible pour toucher l'ennemi ou sa couverture

                AimAngle();
                yield return null;
            }
            yield return null;
        }
    }

    private void InFire()
    {
        StartCoroutine(PlayAnimation());
        SetIsCovered(false);
        IEnumerator PlayAnimation()
        {

            //Jouer une animation
            AnimationClip MyAnimationClip = CoverAnimation.AnimationInFire;
            SelectAnimatorTrueBool("InFire");

            yield return null;

            RandomAim = Random.value;
        }
    }

    #endregion



    // Concerne les tirs et la visée 




    private void Shoot(AnimationEvent myEvent) // Cette fonction s'active dans l'event de l'animation
    {
        if (myEvent.animatorClipInfo.weight > 0.5f)
        {
            LinecastResult = SendLinecastToTargetAndConvertToFilteredList(GetIsCovered());

            AimAngle();
            VisualBulletEffect(); //Effet de ligne atteignant sa cible
            SendDamage(); //Envoie les dégats à ce qui est touché


            LinecastResult = null;
        }
    }

    private Vector2 RandomTargetPositionBetweenBounds()
    {
        if (Target == null)
        {
            //Remplacer cela par un tir général sur la couverture 

            Debug.LogError("Target is null");
            return new Vector2();
        }

        if (TargetBoundsListForAim[0] == null || TargetBoundsListForAim[1] == null)
        {
            TargetBoundsListForAim[0] = Target.transform;
            TargetBoundsListForAim[1] = Target.transform;
        }

        Vector2 VectorDirectionBetweenTargetBounds = TargetBoundsListForAim[1].position - TargetBoundsListForAim[0].position; //Récupère la direction entre les deux boundstarget
        Vector2 RandomTargetPositionBetweenBounds = (Vector2)TargetBoundsListForAim[0].position + RandomAim * VectorDirectionBetweenTargetBounds; //On part de LowerBounds, et on rajoute entre 0 et 1 * la direction vers Upper
        return RandomTargetPositionBetweenBounds;
    }

    private void AimAngle() //Défini l'angle du bras pour lors du tir ou de la visée
    {
        if (Target == null) return; //Ne fonctionne pas sans Target

        //Système de visée
        Transform AimReferencePoint = AimVariables.AimReferencePoint.transform;
        Vector2 TargetPosition = RandomTargetPositionBetweenBounds();
        Vector3 AngleRightDirection = Vector2.right;
        Vector3 AngleLeftDirection = Vector2.left;
        Transform WeaponCanon = ShootVariables.WeaponCanon.transform;

        //Calibration de la visée en fonction de l'arme
        Vector3 Calibration = WeaponCanon.position - AimReferencePoint.position;
        Vector3 TargetWithCalibration = ((Vector3)TargetPosition + Calibration * -1);

        //Calcul de l'angle de joueur vers la cible

        float Angle;
        if (transform.localEulerAngles.y == 0) //Si le joueur regarde vers la droite
            Angle = Vector2.Angle(AngleRightDirection, TargetWithCalibration - AimReferencePoint.position);
        else // Si le joueur regarde vers la gauche
            Angle = Vector2.Angle(AngleLeftDirection, TargetWithCalibration - AimReferencePoint.position);

        //Détermine Angle négatif si la cible est en bas 
        if (TargetWithCalibration.y < AimReferencePoint.position.y) Angle *= -1;

        //return si Angle dépasse l'intervalle de angle direction
        if (Angle < -60 || Angle > 60) return;

        //Défini l'angle de l'animator pour correspondre à l'angle pour viser l'ennemi
        GetComponent<Animator>().SetFloat("angle direction", Angle);
    }


    // Lance une linecast de l'entité jusqu'à la cible, retourne une Liste filtrée
    private List<RaycastHit2D> SendLinecastToTargetAndConvertToFilteredList(bool FilterLinecastFromCover = true)
    {
        Target.TryGetComponent(out StatePhaseEntity TargetIsEntity); //Définis le script de l'Entité
        GameObject TargetCover = null;

        if (TargetIsEntity) TargetCover = TargetIsEntity.GetCover(); //Si une entité, défini Cover de Target pour l'utiliser dans Predicate();
        Transform AimReferencePoint = AimVariables.AimReferencePoint.transform;

        string ThisLayerMask = LayerMask.LayerToName(gameObject.layer);
        LayerMask MyLayerMask = LayerMask.GetMask(ThisLayerMask); //Définis le layermask  de l'entité
        

        var LinecastHitsArray = Physics2D.LinecastAll(AimReferencePoint.position, RandomTargetPositionBetweenBounds(), ~MyLayerMask); // linecast qui va du bras où sont calculés les angles jusqu'à la cible
        List<RaycastHit2D> result = LinecastHitsArray.ToList(); // Convertit LinecastHitsArray en List

        result.RemoveAll(Predicate); //supprime tous les ennemis et targets qui ne sont pas la Target et la Target, si celle ci est en couverture complète

        return result; // on transforme la LinecastHit en List

        bool Predicate(RaycastHit2D GameObjectHit)
        {
            //Si les gameobjects hits sont EnemiesLayer ou TargetableLayer et en plus, si n'est pas la Target
            //On appellera tout le temps cette condition quand la cible est à découvert
            if ((GameObjectHit.transform.gameObject.layer == EnemiesLayer
                || GameObjectHit.transform.gameObject.layer == LayerMask.NameToLayer("Targetable"))
                && GameObjectHit.transform.gameObject != Target) return true;

            //Concerne Target si est une Entity 
            else if (TargetIsEntity)
            {

                //Si les gameObjects hits sont des covers et en plus, ne sont pas la cover de Target, et les arguments de la fonction FilterLinecastFromCover sont sur faux
                //On l'appelle quand on veut toucher la couverture de l'ennemi car la cible est à couvert
                if (GameObjectHit.transform.gameObject.layer == LayerMask.NameToLayer("Cover") && GameObjectHit.transform.gameObject != TargetCover && !FilterLinecastFromCover) return true;


                //Si les gameobjects hits sont la target, et que target est à couvert 
                //On l'appelle quand on ne veut rien toucher car la cible est à couvert
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
        var WeaponCanon = ShootVariables.WeaponCanon.transform;
        var BulletWeaponLineCache = ShootVariables.BulletWeaponLineCache;

        var BulletLineGameObject = Instantiate(BulletWeaponLineCache, new Vector2(), new Quaternion());

        BulletLineGameObject.SetActive(true); // Active le gameObject pour y accéder dans la variable ci dessous + initialiser le GO
        var BulletLine = BulletLineGameObject.GetComponent<LineRenderer>();



        if (LinecastResult.Count > 0)
        {
            RaycastHit2D Target = LinecastResult[0];

            //déplace le transform de BulletLineGameObject et donc du point de référence de LineRenderer pour avoir une destination globale
            BulletLineGameObject.transform.position = new Vector2();

            //Set les points de la line renderer du canon à cible
            BulletLine.SetPosition(0, WeaponCanon.position);
            BulletLine.SetPosition(1, Target.point);
        }

        if (LinecastResult.Count == 0)
        {
            //déplace le transform de BulletLineGameObject et donc du point de référence de LineRenderer pour avoir une destination locale
            BulletLineGameObject.transform.position = WeaponCanon.position;

            //Les Bullets Line prennent les coordonnées locales du BulletLineGameObject + set les points de la line renderer pour tirer en ligne droite
            BulletLine.SetPosition(0, new Vector2());
            BulletLine.SetPosition(1, (WeaponCanon.right * 500));


        }
        StartCoroutine(DestroyBulletLine());

        IEnumerator DestroyBulletLine()
        {
            yield return new WaitForSeconds(0.03f);
            Destroy(BulletLineGameObject);
        }
    }

    private void SendDamage()
    {
        if (LinecastResult.Count == 0) return;
        RaycastHit2D Target = LinecastResult[0];

        print("Send from " + gameObject.name);

        if (Target.transform.TryGetComponent(out IDamageable TargetStatus))
        {
            TargetStatus.ReceiveDamage(DamageInfo);
        }

    }

}
