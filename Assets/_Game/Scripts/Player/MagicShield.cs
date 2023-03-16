using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicShield : MonoBehaviour{
    private static readonly int ShieldColor1 = Shader.PropertyToID("_ShieldColor");

    [SerializeField] private float redirectTime = 0.1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D light2D;
    [SerializeField] private ShieldColor[] shieldColors;
    [SerializeField] private MagicType shieldMagicType;

    private float shieldActivateTime;

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Projectile")){ return; }
        var projectile = col.GetComponent<Projectile>();
        if (projectile.MagicType != shieldMagicType){ return; }
        ReflectProjectile(projectile);
    }

    public void RotateShield(float lookAngle){
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }

    public void ChangeShieldColor(int colorIndex){
        shieldMagicType = (MagicType) colorIndex;
        spriteRenderer.color = shieldColors[colorIndex].SpriteColor;
        light2D.color = shieldColors[colorIndex].LightColor;
        spriteRenderer.material.SetColor(ShieldColor1, shieldColors[colorIndex].EmissionColor);
    }

    public void ToggleShield(bool value){
        gameObject.SetActive(value);
        if (value){ shieldActivateTime = Time.time; }
    }

    private void ReflectProjectile(Projectile projectile){
        bool pressedOnTime = Time.time - shieldActivateTime <= redirectTime;
        if (pressedOnTime){
            Vector3 myPosition = transform.position;
            Vector2 projectileDirection = projectile.transform.position - myPosition;
            Vector2 lookAngle = Quaternion.Euler(0f, 0f, PlayerMovement.LookAngle) * Vector3.right;
            float angle = Vector2.SignedAngle(projectileDirection, lookAngle);
            projectile.transform.RotateAround(myPosition, Vector3.forward, angle);
        }

        bool reflect = false;

        switch (projectile.ProjectileType){
            case ProjectileType.Circle:
                reflect = true;
                break;
            case ProjectileType.Slash:
                if (!projectile.enteredMagicShield && pressedOnTime){ reflect = true; }
                projectile.enteredMagicShield = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (!reflect){ return; }
        projectile.transform.Rotate(Vector3.forward, 180f);
        projectile.reflected = true;
    }

    [Serializable]
    public class ShieldColor{
        [field: SerializeField] public Color SpriteColor{ get; private set; }
        [field: SerializeField] public Color LightColor{ get; private set; }
        [field: SerializeField, ColorUsage(false, true)] public Color EmissionColor{ get; private set; }
    }
}