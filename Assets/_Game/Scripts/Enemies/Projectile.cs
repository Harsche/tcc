using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool random;
    public bool enteredMagicShield;
    public bool reflected;
    [field: SerializeField] public ProjectileType ProjectileType{ get; private set; }
    [field: SerializeField] public MagicType MagicType{ get; private set; }

    private void Awake(){
        Vector2 direction = Player.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (random){
            ChangeColor((MagicType) Random.Range(0, 3));
            return;
        }
        ChangeColor(MagicType.Red);
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
                col.GetComponent<Enemy>().ChangeHp(-damage);
                Destroy(gameObject);
                return;
            case true when col.CompareTag("Button"):
                col.GetComponent<Button>().Interact(MagicType);
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