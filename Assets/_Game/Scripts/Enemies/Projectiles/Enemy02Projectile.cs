using System;
using UnityEngine;

    public class Enemy02Projectile : ProjectileBase{
        protected override void Awake(){
            ChangeColor(MagicType);
        }

        protected override void Update(){
            Rigidbody.rotation = Vector2.Angle(Vector2.right, Rigidbody.velocity);
        }
        
    }