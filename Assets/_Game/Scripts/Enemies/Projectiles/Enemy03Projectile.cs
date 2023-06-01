using UnityEngine;
using Utils;

public class Enemy03Projectile : ProjectileBase{
    protected override void Awake(){
        ChangeColor(MagicType);
    }

    protected override void Start(){
        base.Start();
        Vector2 velocity = GameUtils.GetPlayerDistance(transform.position).normalized * speed;
        Rigidbody.velocity = velocity;
    }
}