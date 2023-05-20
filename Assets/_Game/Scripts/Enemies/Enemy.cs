using System;
using System.Collections;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour{
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

    protected Vector2 startPosition;
    private bool playerInRange;
    private bool playerInSight;
    private Coroutine checkPlayerInRangeCoroutine;
    private StateMachine<State> stateMachine;

    public event Action<Enemy> OnDeath;
    [field: SerializeField] public float Hp{ get; protected set; }
    [field: SerializeField] public float MaxHp{ get; protected set; } = 3f;

    protected bool CheckPlayerInRange{
        get => checkPlayerInRange;
        set{
            checkPlayerInRange = value;
            ToggleCheckPlayerInRange(value);
        }
    }

    protected virtual void Awake(){
        ChangeHp(MaxHp);
        startPosition = transform.position;
        if (useAi){
            stateMachine = new StateMachine<State>(this);
            stateMachine.ChangeState(State.Patrol);
        }
    }

    protected virtual void Start(){
        if (checkPlayerInRange){ checkPlayerInRangeCoroutine = StartCoroutine(CheckPlayerInRangeCoroutine()); }
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

    protected virtual void OnCheckPlayer(bool isInSight){ }

    protected bool GetFloorAhead(){
        if (!checkFloorAhead){ Debug.LogException(new Exception("Not supposed to check for floor ahead.")); }
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

    protected void ChangeBehaviour(State state){
        stateMachine.ChangeState(state);
        currentState = state;
    }
    
    protected bool GetFloorAhead(float xDirection){
        if (!checkFloorAhead){ Debug.LogException(new Exception("Not supposed to check for floor ahead.")); }
        xDirection = (int) Mathf.Sign(xDirection);
        if (isSpriteFlippedX){ xDirection *= -1; }
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

    protected void ToggleCheckPlayerInRange(bool value){
        if (value == (checkPlayerInRangeCoroutine != null)){ return; }
        if (value){
            checkPlayerInRangeCoroutine = StartCoroutine(CheckPlayerInRangeCoroutine());
            return;
        }
        
        StopCoroutine(checkPlayerInRangeCoroutine);
        checkPlayerInRangeCoroutine = null;
    }

    private IEnumerator CheckPlayerInRangeCoroutine(){
        var waitForSeconds = new WaitForSeconds(0.5f);
        while (checkPlayerInRange){
            yield return waitForSeconds;
            Vector2 playerPosition = Player.Instance.transform.position;
            Vector2 myPosition = transform.position;
            Vector2 playerOffset = playerPosition - myPosition;
            playerInRange = playerOffset.magnitude <= maxPlayerDistance;
            if (!checkPlayerInSight){
                OnCheckPlayer(playerInRange);
                continue;
            }
            RaycastHit2D hit2D = Physics2D.Raycast(myPosition, playerOffset, maxPlayerDistance);
            playerInSight = hit2D.collider && hit2D.collider.CompareTag("Player");
            OnCheckPlayer(playerInRange && playerInSight);
        }
    }

#if UNITY_EDITOR
    [SerializeField] protected bool checkFloorAhead;

    private void OnValidate(){
        CheckPlayerInRange = checkPlayerInRange;
        if (checkFloorAhead && !floorCheckOrigin){
            floorCheckOrigin = new GameObject("Floor Check Origin").transform;
            floorCheckOrigin.SetParent(transform);
            floorCheckOrigin.localPosition = Vector2.one;
        }
    }
#endif
}