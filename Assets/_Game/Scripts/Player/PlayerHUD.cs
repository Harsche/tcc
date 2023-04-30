using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image absorbedColor;
    public static PlayerHUD Instance{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateHpBar(){
        hpSlider.value = Player.Instance.Hp / Player.Instance.MaxHp;
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