using System;
using System.Collections;
using UnityEngine;

public class Enemy03 : MonoBehaviour{
    [SerializeField] private bool invulnerable;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private bool checkPlayerDistance = true;
    public Transform projectilePrefab;
    private Coroutine attackCoroutine;
    private Coroutine checkPlayerDistanceCoroutine;

    public event Action<Enemy03> OnDeath;
    [field: SerializeField] public float Hp{ get; private set; }
    [field: SerializeField] public float MaxHp{ get; private set; } = 3f;

    private void Awake(){
        if (checkPlayerDistance){ checkPlayerDistanceCoroutine = StartCoroutine(CheckPlayerDistance()); }
        else{ attackCoroutine = StartCoroutine(AttackCoroutine()); }
        ChangeHp(MaxHp);
    }

    private void OnDestroy(){
        OnDeath = null;
    }

    private IEnumerator CheckPlayerDistance(){
        var waitForSeconds = new WaitForSeconds(1f);
        while (true){
            yield return waitForSeconds;
            Vector2 playerPosition = Player.Instance.transform.position;
            bool withinRange = Vector2.Distance(playerPosition, transform.position) <= attackDistance;
            if (withinRange){ attackCoroutine ??= StartCoroutine(AttackCoroutine()); }
            else if (attackCoroutine != null){
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackCoroutine(){
        WaitForSeconds waitForSeconds = new(attackCooldown);
        while (true){
            yield return waitForSeconds;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void ChangeHp(float value){
        if (invulnerable){ return; }
        Hp = Mathf.Clamp(Hp + value, 0f, MaxHp);
        if (Hp != 0){ return; }
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}