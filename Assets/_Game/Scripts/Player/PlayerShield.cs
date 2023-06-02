using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShield : MonoBehaviour{
    [SerializeField] private int maxHits = 3;
    [field: SerializeField] public int CurrentHp{ get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [FormerlySerializedAs("collider2D"), SerializeField] private Collider2D shieldCollider2D;
    public bool unlocked;

    private void Awake(){
        ToggleShield(false);
    }

    private void Start(){
        ChangeShieldHp(maxHits);
    }

    private void OnEnable(){
        PlayerInput.OnShieldToggle += ToggleShield;
    }

    private void OnDisable(){
        PlayerInput.OnShieldToggle -= ToggleShield;
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Projectile")){ return; }
        ChangeShieldHp(-1);
        Destroy(col.gameObject);
    }

    private void ToggleShield(bool value){
        if (value && !unlocked){ return; }
        
        if (value){
            if (CurrentHp <= 0){ return; }
            spriteRenderer.enabled = true;
            shieldCollider2D.enabled = true;
            return;
        }
        spriteRenderer.enabled = false;
        shieldCollider2D.enabled = false;
    }

    private void ChangeShieldHp(int value){
        CurrentHp += value;
        PlayerHUD.Instance.UpdateShield(CurrentHp);
        if (CurrentHp == 0){ ToggleShield(false); }
    }
}