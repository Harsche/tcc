using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool random;
    [SerializeField] private bool fixedDirection;
    [SerializeField] private EnemyAttackDirection enemyAttackDirection;
    public bool enteredMagicShield;
    public bool reflected;
    [field: SerializeField] public ProjectileType ProjectileType{ get; private set; }
    [field: SerializeField] public MagicType MagicType{ get; private set; }

    private void Awake(){
        Vector2 targetDirection;
        if (fixedDirection){
            float fixedAngle = 90f * (int) enemyAttackDirection;
            targetDirection = Quaternion.AngleAxis(fixedAngle, Vector3.forward) * Vector2.right;
        }
        else{ targetDirection = Player.Instance.transform.position - transform.position; }
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (random){
            ChangeColor((MagicType) Random.Range(0, 3));
            return;
        }
        ChangeColor(MagicType);
        Destroy(gameObject, destroyTime);
    }

    private void Update(){
        transform.Translate(Vector2.right * (speed * Time.deltaTime), Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Player")){
            col.GetComponent<Player>().ChangeHp(-damage);
            Destroy(gameObject);
            return;
        }
        switch (reflected){
            case true when col.CompareTag("Enemy"):
                col.GetComponent<Enemy03>().ChangeHp(-damage);
                Destroy(gameObject);
                return;
            case true when col.CompareTag("Button"):
                col.GetComponent<GameButton>().Interact(MagicType);
                Destroy(gameObject);
                break;
        }
    }

    public void ChangeColor(MagicType magicType){
        MagicType = magicType;
        Color projectileColor = magicType switch{
            MagicType.Red => Color.red,
            MagicType.Green => Color.green,
            MagicType.Blue => Color.blue,
            _ => throw new ArgumentOutOfRangeException(nameof(magicType), magicType, null)
        };
        spriteRenderer.color = projectileColor;
    }
}