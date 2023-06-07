using System;
using Data.ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Parry : MonoBehaviour{
    private static readonly int ShieldColor1 = Shader.PropertyToID("_ShieldColor");

    [SerializeField] private float redirectImpulseAngle = 45f;
    [SerializeField] private float reflectImpulse = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D light2D;
    [FormerlySerializedAs("shieldMagicType")] [SerializeField] private Element shieldElement;
    [SerializeField] private Collider2D shieldCollider;
    public bool enableGreen = true;
    public bool enableRed;
    public bool enableBlue;
    private bool enableParry;
    private bool isParrying;
    private Coroutine parryCoroutine;


    private float shieldActivateTime;

    public event Action OnParry;

    public bool EnableParry{
        get => enableParry;
        set{
            PlayerHUD.Instance.ToggleStaffHUDElement(value);
            enableParry = value;
        }
    }

    private void Awake(){
        ToggleParry(false);
    }

    private void Start(){
        Player.Instance.PlayerAnimation.OnParry += () => ToggleParry(true);
        ChangeParryColor(Element.Nature);
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
        var projectile = col.GetComponent<ProjectileBase>();
        if (projectile.Element != shieldElement){ return; }
        ReflectProjectile(projectile);
    }

    public void CancelParry(){
        isParrying = false;
        ToggleParry(false);
    }

    private void ChangeParryColor(Element element){
        switch (element){
            case (Element) 1:
                if (!enableGreen){ return; }
                break;
            case (Element) 2:
                if (!enableRed){ return; }
                break;
            case (Element) 3:
                if (!enableBlue){ return; }
                break;
        }
        GameData.ElementData elementData = GameManager.GameData.elementsData[element];
        shieldElement = element;
        spriteRenderer.color = elementData.SpriteColor;
        light2D.color = elementData.LightColor;
        spriteRenderer.material.SetColor(ShieldColor1, elementData.EmissionColor);
        Player.Instance.PlayerAnimation.ChangeParryColor(elementData.SpriteColor);
        PlayerHUD.Instance.SetParryColor(elementData.SpriteColor);
    }

    private void ReflectProjectile(ProjectileBase projectile){
        // Update other features with the element of the refleted projectile
        Player.Instance.ElementMagic.hasAbsorbedElement = true;
        PlayerHUD.Instance.SetAbsorbedElement(Element.None);
        Player.Instance.ElementMagic.absorbedElement = projectile.Element;
        PlayerHUD.Instance.SetAbsorbedElement(projectile.Element);
        GameData.ElementData elementData = GameManager.GameData.elementsData[projectile.Element];
        Player.Instance.PlayerAnimation.ChangeEyesColor(elementData.SpriteColor);

        // Reflecting the projectile
        Vector2 myPosition = transform.position;
        Vector2 projectileDirection = projectile.Rigidbody.position - myPosition;
        Vector2 lookAngle = PlayerInput.LookDirection;
        Vector2 reflectedPosition = myPosition + lookAngle * projectileDirection.magnitude;
        projectile.Rigidbody.MovePosition(reflectedPosition);
        projectile.Rigidbody.velocity = lookAngle * projectile.Rigidbody.velocity.magnitude * reflectImpulse;

        // Apply upward jump if reflected from below
        if (!projectile.enteredMagicShield){
            if (Vector2.Angle(Vector2.down, projectileDirection) <= redirectImpulseAngle){
                PlayerMovement playerMovement = Player.Instance.PlayerMovement;
                float height = Player.Instance.PlayerMovement.JumpHeight;
                playerMovement.ForceJump(height);
            }
        }
        projectile.enteredMagicShield = true;
        projectile.reflected = true;
    }

    private void RotateShield(float lookAngle){
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }

    private void StartParry(){
        if (!EnableParry || isParrying){ return; }
        isParrying = true;
        OnParry?.Invoke();
    }

    private void ToggleParry(bool value){
        shieldCollider.enabled = value;
        light2D.enabled = false;
    }


    [Serializable]
    public class ShieldColor{
        [field: SerializeField] public Element Element{ get; private set; }
        [field: SerializeField] public Color SpriteColor{ get; private set; }
        [field: SerializeField] public Color LightColor{ get; private set; }
        [field: SerializeField] [field: ColorUsage(false, true)] public Color EmissionColor{ get; private set; }
    }
}