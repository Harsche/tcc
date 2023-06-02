using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TrailRenderer), typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour{
    [SerializeField] private bool destroyOnHitAnything = true;
    [SerializeField] private int damage = 1;
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private bool random;
    [SerializeField] private bool fixedDirection;
    [SerializeField] private EnemyAttackDirection enemyAttackDirection;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Rigidbody2D myRigidbody;
    public bool enteredMagicShield;
    public bool reflected, hitPlayer;
    [field: SerializeField] public ProjectileType ProjectileType{ get; private set; }
    [field: SerializeField] public Element Element{ get; private set; }

    public Rigidbody2D Rigidbody => myRigidbody;

    protected virtual void Awake(){
        Vector2 targetDirection;
        if (fixedDirection){
            float fixedAngle = 90f * (int) enemyAttackDirection;
            targetDirection = Quaternion.AngleAxis(fixedAngle, Vector3.forward) * Vector2.right;
        }
        else{ targetDirection = Player.Instance.transform.position - transform.position; }
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (random){
            ChangeColor((Element) Random.Range(0, 3));
            return;
        }
        ChangeColor(Element);
        Destroy(gameObject, destroyTime);
    }

    protected virtual void Start(){
        tag = "Projectile";
    }

    protected virtual void OnTriggerEnter2D(Collider2D col){
        if(destroyOnHitAnything && !col.CompareTag("PlayerShield") && !col.CompareTag("Enemy")){Destroy(gameObject);}
        if (col.CompareTag("Player") && !hitPlayer){
            col.GetComponent<Player>().ChangeHp(-damage);
            hitPlayer = true;
            Destroy(gameObject);
            return;
        }
        switch (reflected){
            case true when col.CompareTag("Enemy"):
                col.GetComponent<EnemyBase>().ChangeHp(-damage);
                Destroy(gameObject);
                return;
            case true when col.CompareTag("Button"):
                col.GetComponent<GameButton>().Interact(Element);
                Destroy(gameObject);
                return;
        }
    }

    protected void ChangeColor(Element element){
        Color projectileColor = GameManager.GameData.elementsData[element].SpriteColor;
        trailRenderer.material.color = projectileColor;
    }

#if UNITY_EDITOR
    private void OnValidate(){
        if (!trailRenderer){ trailRenderer = GetComponent<TrailRenderer>(); }
        if (!myRigidbody){ myRigidbody = GetComponent<Rigidbody2D>(); }
    }
#endif
}