using System;
using System.Collections;
using Scripts.Camera;
using UnityEngine;

public class Enemy03 : EnemyBase{
    [Label("Enemy", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)] [SerializeField]
    private EnemyType enemyType;

    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private Transform attackSpawn;
    public Transform projectilePrefab;
    private Coroutine checkPlayerDistanceCoroutine;
    private float lastAttackTime;

    public override event Action OnAttack;

    public void SpawnAttack(){
        Instantiate(projectilePrefab, attackSpawn.position, Quaternion.identity);
    }

    // TODO - Put this feature on EnemyBase or make a manager
    private IEnumerator OnCheckPlayerCoroutine(){
        WaitForSeconds waitTime = new(0.1f);
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

    private void Attack_Enter(){
        GameCamera.Instance.AddTarget(transform);
    }

    private void Attack_OnCheckPlayer(bool isWithinSight){
        if (!isWithinSight){
            ChangeState(State.Patrol);
            return;
        }
        
        enemyAnimation.FacePlayerDirection();
        if (!(Time.time >= lastAttackTime + attackCooldown)){ return; }
        OnAttack?.Invoke();
        lastAttackTime = Time.time;
    }
    
    private void Attack_Exit(){
        GameCamera.Instance.RemoveTarget(transform);
    }

    // ReSharper restore UnusedMember.Local
}