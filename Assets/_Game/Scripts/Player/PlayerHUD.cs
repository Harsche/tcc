using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour{
    [SerializeField] private Slider hpSlider;
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
}