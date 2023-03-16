using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour{
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private Transform projectilePrefab;
    private Coroutine attackCoroutine;
    [field: SerializeField] public float Hp{ get; private set; }
    [field: SerializeField] public float MaxHp{ get; private set; } = 3f;

    private void Awake(){
        attackCoroutine = StartCoroutine(AttackCoroutine());
        ChangeHp(MaxHp);
    }

    private void OnDestroy(){
        OnDeath = null;
    }

    public event Action<Enemy> OnDeath;

    private IEnumerator AttackCoroutine(){
        WaitForSeconds waitForSeconds = new(attackCooldown);
        while (true){
            yield return waitForSeconds;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void ChangeHp(float value){
        Hp = Mathf.Clamp(Hp + value, 0f, MaxHp);
        if (Hp != 0){ return; }
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}