using System;
using UnityEngine;

public class Player : MonoBehaviour{
    [field: SerializeField] public float MaxHp{ get; private set; } = 10f;
    [field: SerializeField] public float Hp{ get; private set; }
    public static Player Instance{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start(){
        ChangeHp(MaxHp);
    }

    public void ChangeHp(float value){
        Hp = Mathf.Clamp(Hp + value, 0f, MaxHp);
        PlayerHUD.Instance.UpdateHpBar();
    }
}