using UnityEngine;
using Utils;

public class Enemy03Projectile : ProjectileBase{

    protected override void Start(){
        ChangeColor(Element);
        Vector2 velocity = GameUtils.GetPlayerDistance(transform.position).normalized * speed;
        Rigidbody.velocity = velocity;
    }
}