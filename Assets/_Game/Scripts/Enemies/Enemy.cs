using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour{
    protected static readonly int Attack = Animator.StringToHash("Attack");

    [SerializeField] protected Vector2 maxSpeed;
    [SerializeField] protected bool isSpriteFlippedX;
    [SerializeField] protected bool invulnerable;
    [SerializeField] protected bool checkPlayerInRange;
    [SerializeField] protected float maxPlayerDistance = 10f;
    [SerializeField] protected bool checkPlayerInSight;
    [SerializeField] protected bool onBattle;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform floorCheckOrigin;
    [SerializeField] protected Rigidbody2D myRigidbody2D;
    private Coroutine checkPlayerInRangeCoroutine;

    private bool playerInRange;
    private bool playerInSight;

    public event Action<Enemy> OnDeath;
    [field: SerializeField] public float Hp{ get; protected set; }
    [field: SerializeField] public float MaxHp{ get; protected set; } = 3f;

    protected virtual void Awake(){
        if (checkPlayerInRange){ checkPlayerInRangeCoroutine = StartCoroutine(CheckPlayerInRangeCoroutine()); }
        ChangeHp(MaxHp);
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

    protected bool GetFloorAhead(){
        if (!checkFloorAhead){
            Exception exception = new("Not supposed to check for floor ahead.");
            Debug.LogException(exception);
        }
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

    protected virtual void OnCheckPlayer(bool isInSight){ }

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
        if (checkFloorAhead && !floorCheckOrigin){
            floorCheckOrigin = new GameObject("Floor Check Origin").transform;
            floorCheckOrigin.SetParent(transform);
            floorCheckOrigin.localPosition = Vector2.one;
        }
    }
#endif
}