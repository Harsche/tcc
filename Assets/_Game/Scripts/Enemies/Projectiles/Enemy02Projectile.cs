using System;
using UnityEngine;

    public class Enemy02Projectile : MonoBehaviour{
        private void OnTriggerEnter2D(Collider2D other){
            if (other.CompareTag("Player")){
                Player.Instance.ChangeHp(-1);
            }
            Destroy(gameObject);
        }
    }
