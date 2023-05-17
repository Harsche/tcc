using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Parry : MonoBehaviour{
    private static readonly int ShieldColor1 = Shader.PropertyToID("_ShieldColor");

    [SerializeField] private float redirectImpulseAngle = 45f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D light2D;
    [SerializeField] private ShieldColor[] shieldColors;
    [SerializeField] private MagicType shieldMagicType;
    [SerializeField] private Collider2D shieldCollider;
    public bool enableParry;
    public bool enableGreen = true;
    public bool enableRed;
    public bool enableBlue;
    private bool isParrying;
    private Coroutine parryCoroutine;


    private float shieldActivateTime;

    public event Action OnParry;

    private void Awake(){
        ToggleShield(false);
    }

    private void Start(){
        Player.Instance.PlayerAnimation.OnParry += () => ToggleShield(true);
        ChangeParryColor(0);
    }

    private void Update(){
        RotateShield(PlayerInput.LookAngle);
    }

    private void OnEnable(){
        // Subscribe to input events
        PlayerInput.OnFireInput += StartParry;
        PlayerInput.OnChangeShieldColor += ChangeParryColor;
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Projectile")){ return; }
        var projectile = col.GetComponent<Projectile>();
        if (projectile.MagicType != shieldMagicType){ return; }
        ReflectProjectile(projectile);
    }

    public void CancelParry(){
        isParrying = false;
        ToggleShield(false);
    }

    private void RotateShield(float lookAngle){
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }

    private void ChangeParryColor(int colorIndex){
        switch (colorIndex){
            case 0:
                if(!enableGreen){return;}
                break;
            case 1:
                if(!enableRed){return;}
                break;
            case 2:
                if(!enableBlue){return;}
                break;
        }
        shieldMagicType = shieldColors[colorIndex].MagicType;
        spriteRenderer.color = shieldColors[colorIndex].SpriteColor;
        light2D.color = shieldColors[colorIndex].LightColor;
        spriteRenderer.material.SetColor(ShieldColor1, shieldColors[colorIndex].EmissionColor);
        Player.Instance.PlayerAnimation.ChangeParryColor(shieldColors[colorIndex].SpriteColor);
        PlayerHUD.Instance.SetParryColor(shieldColors[colorIndex].SpriteColor);
    }

    private void StartParry(){
        if (!enableParry || isParrying){ return; }
        isParrying = true;
        OnParry?.Invoke();
    }

    private void ToggleShield(bool value){
        shieldCollider.enabled = value;
        spriteRenderer.enabled = value;
        light2D.enabled = false;
    }

    private void ReflectProjectile(Projectile projectile){
        Player.Instance.ElementMagic.hasAbsorbedElement = true;
        PlayerHUD.Instance.ToggleAbsorbedElement(true);
        Player.Instance.ElementMagic.absorbedElement = projectile.MagicType;
        PlayerHUD.Instance.SetAbsorbedElement(projectile.MagicType);

        // Rotates projectile around player to adjust reflect direction
        Vector3 myPosition = transform.position;
        Vector2 projectileDirection = projectile.transform.position - myPosition;
        Vector2 lookAngle = Quaternion.Euler(0f, 0f, PlayerInput.LookAngle) * Vector3.right;
        float angle = Vector2.SignedAngle(projectileDirection, lookAngle);
        projectile.transform.RotateAround(myPosition, Vector3.forward, angle);
        projectile.transform.rotation = Quaternion.Euler(Vector3.forward * PlayerInput.LookAngle);
        
        if (!projectile.enteredMagicShield){
            if (Vector2.Angle(Vector2.down, projectileDirection) <= redirectImpulseAngle){
                PlayerMovement playerMovement = Player.Instance.PlayerMovement;
                playerMovement.ForceDash(Vector2.up);
            }
        }
        projectile.enteredMagicShield = true;


        // switch (projectile.ProjectileType){
        //     case ProjectileType.Circle:
        //         // if (!pressedOnTime){ projectile.transform.Rotate(Vector3.forward, 180f); }
        //         break;
        //     case ProjectileType.Slash:
        //         if (!projectile.enteredMagicShield){
        //             if (Vector2.Angle(Vector2.down, projectileDirection) <= redirectImpulseAngle){
        //                 PlayerMovement playerMovement = Player.Instance.PlayerMovement;
        //                 playerMovement.ForceDash(Vector2.up);
        //             }
        //         }
        //         projectile.enteredMagicShield = true;
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }

        projectile.reflected = true;
    }


    [Serializable]
    public class ShieldColor{
        [field: SerializeField] public MagicType MagicType{ get; private set; }
        [field: SerializeField] public Color SpriteColor{ get; private set; }
        [field: SerializeField] public Color LightColor{ get; private set; }
        [field: SerializeField, ColorUsage(false, true)] public Color EmissionColor{ get; private set; }
    }
}