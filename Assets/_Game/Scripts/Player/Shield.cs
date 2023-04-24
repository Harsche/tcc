using UnityEngine;
using UnityEngine.Serialization;

public class Shield : MonoBehaviour{
    [SerializeField] private int maxHits = 3;
    [SerializeField] private int currentHp;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [FormerlySerializedAs("collider2D"),SerializeField] private Collider2D shieldCollider2D;

    private void Awake(){
        currentHp = maxHits;
        ToggleShield(false);
    }

    private void OnEnable(){
        PlayerInput.OnShieldToggle += ToggleShield;
    }

    private void OnDisable(){
        PlayerInput.OnShieldToggle -= ToggleShield;
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Projectile")){ return; }
        currentHp--;
        if(currentHp == 0){ToggleShield(false);}
        Destroy(col.gameObject);
    }

    private void ToggleShield(bool value){
        if (value){
            if (currentHp <= 0){ return; }
            spriteRenderer.enabled = true;
            shieldCollider2D.enabled = true;
            return;
        }
        spriteRenderer.enabled = false;
        shieldCollider2D.enabled = false;
    }
}