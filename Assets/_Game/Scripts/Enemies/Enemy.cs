using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour{
    [SerializeField] protected bool invulnerable;
    [SerializeField] protected bool checkPlayerInRange;
    [SerializeField] protected float maxPlayerDistance = 10f;
    [SerializeField] protected bool checkPlayerInSight;

    private bool playerInRange;
    private bool playerInSight;
    private Coroutine checkPlayerInRangeCoroutine;

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

    private IEnumerator CheckPlayerInRangeCoroutine(){
        var waitForSeconds = new WaitForSeconds(0.5f);
        while (checkPlayerInRange){
            yield return waitForSeconds;
            Vector2 playerPosition = Player.Instance.transform.position;
            Vector2 myPosition = transform.position;
            Vector2 playerOffset = playerPosition - myPosition;
            playerInRange = playerOffset.magnitude <= maxPlayerDistance;
            if (!checkPlayerInSight){ continue; }
            RaycastHit2D hit2D = Physics2D.Raycast(myPosition, playerOffset, maxPlayerDistance);
            playerInSight = hit2D.collider && hit2D.collider.CompareTag("Player");
        }
    }
}