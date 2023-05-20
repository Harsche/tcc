using System;
using Game.AI;
using MonsterLove.StateMachine;
using UnityEngine;

public class EnemyBase : MonoBehaviour{
    protected static readonly int Attack = Animator.StringToHash("Attack");

    [SerializeField] protected bool useAi;
    [SerializeField] protected State currentState;
    [SerializeField] protected Vector2 maxSpeed;
    [SerializeField] protected bool isSpriteFlippedX;
    [SerializeField] protected bool invulnerable;
    [SerializeField] private bool checkPlayerInRange;
    [SerializeField] protected float maxPlayerDistance = 10f;
    [SerializeField] protected bool checkPlayerInSight;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform floorCheckOrigin;
    [SerializeField] protected Rigidbody2D myRigidbody2D;
    private Coroutine checkPlayerInRangeCoroutine;
    private bool playerInRange;
    private bool playerInSight;

    protected Vector2 startPosition;
    protected bool isAttacking;
    private StateMachine<State, EnemyDriver> stateMachine;

    public event Action<EnemyBase> OnDeath;
    [field: SerializeField] public float Hp{ get; protected set; }
    [field: SerializeField] public float MaxHp{ get; protected set; } = 3f;

    protected EnemyDriver StateMachineDriver => stateMachine.Driver;

    protected virtual void Awake(){
        ChangeHp(MaxHp);
        startPosition = transform.position;
        if (useAi){
            stateMachine = new StateMachine<State, EnemyDriver>(this);
            stateMachine.ChangeState(State.Patrol);
        }
    }

    protected virtual void Update(){
        StateMachineDriver.Update.Invoke();
    }

    protected virtual void OnDestroy(){
        OnDeath = null;
    }

    public void ChangeHp(float value){
        if (invulnerable){ return; }
        Hp = Mathf.Clamp(Hp + value, 0f, MaxHp);
        if (Hp != 0){ return; }
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void SetAttackingFinished(){
        isAttacking = false;
    }

    protected bool GetFloorAhead(){
#if UNITY_EDITOR
        if (!checkFloorAhead){ Debug.LogException(new Exception("Not supposed to check for floor ahead.")); }
#endif
        int facingDirection = spriteRenderer.flipX ? -1 : 1;
        if (isSpriteFlippedX){ facingDirection *= -1; }
        Vector2 origin = floorCheckOrigin.localPosition;
        origin.x = Mathf.Abs(origin.x) * facingDirection;
        floorCheckOrigin.localPosition = origin;
        RaycastHit2D hit2D = Physics2D.Raycast(
            floorCheckOrigin.position,
            Vector2.down, 1.3f,
            LayerMask.GetMask("Ground")
        );
        return hit2D.collider;
    }

    protected void ChangeState(State state){
        currentState = state;
        stateMachine.ChangeState(state);
    }

    protected bool GetFloorAhead(float xDirection){
#if UNITY_EDITOR
        if (!checkFloorAhead){ Debug.LogException(new Exception("Not supposed to check for floor ahead.")); }
#endif
        xDirection = (int) Mathf.Sign(xDirection);
        Vector2 origin = floorCheckOrigin.localPosition;
        origin.x = Mathf.Abs(origin.x) * xDirection;
        floorCheckOrigin.localPosition = origin;
        RaycastHit2D hit2D = Physics2D.Raycast(
            floorCheckOrigin.position,
            Vector2.down, 1.3f,
            LayerMask.GetMask("Ground")
        );
        return hit2D.collider;
    }

    protected bool CheckPlayerInRange(){
        Vector2 playerPosition = Player.Instance.transform.position;
        Vector2 myPosition = transform.position;
        Vector2 playerOffset = playerPosition - myPosition;
        playerInRange = playerOffset.magnitude <= maxPlayerDistance;
        if (!checkPlayerInSight){ return playerInRange; }
        RaycastHit2D hit2D = Physics2D.Raycast(myPosition, playerOffset, maxPlayerDistance);
        playerInSight = hit2D.collider && hit2D.collider.CompareTag("Player");
        return playerInSight;
    }


#if UNITY_EDITOR
    [SerializeField] protected bool checkFloorAhead;

    private void OnValidate(){
        if (checkFloorAhead && !floorCheckOrigin){
            floorCheckOrigin = new GameObject("Floor Check Origin").transform;
            floorCheckOrigin.SetParent(transform);
            floorCheckOrigin.localPosition = Vector2.one;
        }
    }
#endif
}