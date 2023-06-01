using System;
using System.Collections;
using UnityEngine;

public class Enemy01 : EnemyBase{
    [Label("Enemy", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)] [SerializeField]
    private float rayDistance = 0.25f;

    [SerializeField] private Vector2 rayOffset;

    public override event Action OnAttack;

    protected override void Awake(){
        base.Awake();
        StartCoroutine(OnCheckPlayerCoroutine());
    }


    private IEnumerator OnCheckPlayerCoroutine(){
        WaitForSeconds waitTime = new(0.1f);
        while (gameObject){
            yield return waitTime;
            StateMachineDriver.OnCheckPlayer.Invoke(CheckPlayerInRange());
        }
    }


    // ReSharper disable UnusedMember.Local
    private void Patrol_Enter(){
        myRigidbody2D.velocity = maxSpeed;
    }

    private void Patrol_OnCheckPlayer(bool isPlayerInSight){
        Vector2 origin = myRigidbody2D.position + new Vector2(rayOffset.x * FacingDirection, rayOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.right * FacingDirection,
            rayDistance,
            LayerMask.GetMask("Ground")
        );
        if (hit.collider){ myRigidbody2D.velocity *= -1f; }
    }

    // ReSharper restore UnusedMember.Local
}