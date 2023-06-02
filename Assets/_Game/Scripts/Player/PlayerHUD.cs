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

    /// <summary>
    /// Updates the slider on the HUD that represents the hit points of the Player.
    /// </summary>
    /// <param name="playerCurrentHp"></param>
    public void UpdateHp(int playerCurrentHp){
        for (int i = 0; i < hpPetals.Length; i++){
            hpPetals[i].enabled = i + 1 <= playerCurrentHp;
        }
    }
    
    /// <summary>
    /// Updates the slider on the HUD that represents the hit points of the Player's shield.
    /// </summary>
    /// <param name="shieldCurrentHp"></param>
    public void UpdateShield(int shieldCurrentHp){
        for (int i = 0; i < shieldCounts.Length; i++){
            shieldCounts[i].enabled = i + 1 <= shieldCurrentHp;
        }
    }
    
    /// <summary>
    /// Updates the image on the HUD that represents the color of the
    /// currently selected element.
    /// </summary>
    /// <param name="color"></param>
    public void SetParryColor(Color color){
        parryColor.color = color;
    }

    /// <summary>
    /// Updates the image on the HUD that represents the color of the
    /// currently absorbed element.
    /// </summary>
    /// <param name="element"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetAbsorbedElement(Element element){
        Color color = element switch{
            Element.Blue => Color.blue,
            Element.Red => Color.red,
            Element.Green => Color.green,
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };
        absorbedColor.color = color;
    }
    
    // TODO: Delete and substitute with no element (White)
    public void ToggleAbsorbedElement(bool value){
        if(!value){absorbedColor.color = Color.white;}
        
    }
}