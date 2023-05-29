using System;
using System.Collections;
using UnityEngine;

public class Enemy03 : EnemyBase{
    [Label("Enemy", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)] [SerializeField]
    private EnemyType enemyType;

    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float attackDistance = 5f;
    public Transform projectilePrefab;
    private Coroutine checkPlayerDistanceCoroutine;
    private float lastAttackTime;

    public override event Action OnAttack;

    // TODO - Put this feature on EnemyBase or make a manager
    private IEnumerator OnCheckPlayerCoroutine(){
        WaitForSeconds waitTime = new(0.25f);
        while (gameObject){
            yield return waitTime;
            StateMachineDriver.OnCheckPlayer.Invoke(CheckPlayerInRange());
        }
    }

    // ReSharper disable UnusedMember.Local
    // State Machines Methods are called only at runtime

    private void Patrol_Enter(){
        checkPlayerDistanceCoroutine = StartCoroutine(OnCheckPlayerCoroutine());
    }

    private void Patrol_OnCheckPlayer(bool isWithinSight){
        bool canAttack = Utils.GameUtils.GetPlayerDistance(transform.position).magnitude <= attackDistance;
        if (isWithinSight && canAttack){ ChangeState(State.Attack); }
    }

    private void Attack_Update(){
        if (!(Time.time >= lastAttackTime + attackCooldown)){ return; }
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        lastAttackTime = Time.time;
    }

    private void Attack_OnCheckPlayer(bool isWithinSight){
        if (!isWithinSight){ ChangeState(State.Patrol); }
    }

    // ReSharper restore UnusedMember.Local
}