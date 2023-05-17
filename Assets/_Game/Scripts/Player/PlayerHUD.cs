using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour{
    [SerializeField] private Image[] hpPetals;
    [SerializeField] private Image[] shieldCounts;
    [SerializeField] private Image absorbedColor;
    [SerializeField] private Image parryColor;
    public static PlayerHUD Instance{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateHp(int playerCurrentHp){
        for (int i = 0; i < hpPetals.Length; i++){
            hpPetals[i].enabled = i + 1 <= playerCurrentHp;
        }
    }
    
    public void UpdateShield(int shieldCurrentHp){
        for (int i = 0; i < shieldCounts.Length; i++){
            shieldCounts[i].enabled = i + 1 <= shieldCurrentHp;
        }
    }
    
    public void SetParryColor(Color color){
        parryColor.color = color;
    }

    public void SetAbsorbedElement(MagicType magicType){
        Color color = magicType switch{
            MagicType.Blue => Color.blue,
            MagicType.Red => Color.red,
            MagicType.Green => Color.green,
            _ => throw new ArgumentOutOfRangeException(nameof(magicType), magicType, null)
        };
        absorbedColor.color = color;
    }

    public void ToggleAbsorbedElement(bool value){
        absorbedColor.enabled = value;
    }
}