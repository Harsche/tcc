using System.Collections;
using UnityEngine;

public class Enemy02 : Enemy{
    [SerializeField] private float desiredAttackDistance = 4f;
    [SerializeField] private float attackRecoverTime = 2f;
    private Coroutine attackCoroutine;

    protected override void Awake(){
        base.Awake();
    }

    protected override void OnCheckPlayer(bool isInSight){
        onBattle = isInSight;
        if (!isInSight){
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            return;
        }
        attackCoroutine ??= StartCoroutine(MainAttackCoroutine());
    }

    private IEnumerator MainAttackCoroutine(){
        while (true){
            Vector2 playerDistance = Utils.GameUtils.GetPlayerDistance(transform.position);
            bool attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;

            if (attack){
                animator.SetTrigger(Attack);
                yield return new WaitForSeconds(attackRecoverTime);
            }

            Vector2 followPlayerSpeed = maxSpeed;
            followPlayerSpeed.x *= Mathf.Sign(playerDistance.x);
            // myRigidbody2D.velocity = Utils.GameUtils.OrientVelocityToGround(followPlayerSpeed, );
        }
    }
}