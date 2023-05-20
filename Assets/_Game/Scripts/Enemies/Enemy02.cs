using System;
using System.Collections;
using UnityEngine;

public class Enemy02 : Enemy{
    [SerializeField] private float desiredAttackDistance = 4f;
    [SerializeField] private float attackRecoverTime = 2f;

    protected override void OnCheckPlayer(bool isInSight){
        if(!useAi) {Debug.Log(new Exception("Enemy is not configured to use AI."));}
        switch (isInSight){
            case true when currentState != State.Chase:
                ChangeBehaviour(State.Chase);
                return;
            case false when currentState is State.Attack or State.Chase:
                ChangeBehaviour(State.Patrol);
                break;
        }
    }

    private void Chase_Update(){
        Vector2 playerDistance = Utils.GameUtils.GetPlayerDistance(transform.position);
        bool attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;
        if (attack){
            ChangeBehaviour(State.Attack);
            return;
        }

        if (GetFloorAhead(playerDistance.x)){
            myRigidbody2D.velocity = Vector2.zero;
            return;
        }

        Vector2 velocity = myRigidbody2D.velocity;
        velocity.x = maxSpeed.x * Mathf.Sign(playerDistance.x);
        myRigidbody2D.velocity = velocity;
    }

    private IEnumerator BehaviourCoroutine(){
        while (gameObject){
            Vector2 playerDistance = Utils.GameUtils.GetPlayerDistance(transform.position);
            bool attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;

            while (attack){
                animator.SetTrigger(Attack);
                myRigidbody2D.velocity = Vector2.zero;
                yield return new WaitForSeconds(attackRecoverTime);
                playerDistance = Utils.GameUtils.GetPlayerDistance(transform.position);
                attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;
            }

            Vector2 followPlayerSpeed = maxSpeed;
            followPlayerSpeed.x *= Mathf.Sign(playerDistance.x);
            // myRigidbody2D.velocity = Utils.GameUtils.OrientVelocityToGround(followPlayerSpeed, );
        }
    }

    // private IEnumerator Patrol(){
    //     
    // }
}