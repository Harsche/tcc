using UnityEngine;

public class Player : MonoBehaviour{
    [field: SerializeField] public float MaxHp{ get; private set; } = 10f;
    [field: SerializeField] public float Hp{ get; private set; }
    [field: SerializeField] public Camera playerCamera{ get; private set; }
    public static Player Instance{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        GameSaveSystem.OnLoadSave += LoadPlayerData;
    }

    private void Start(){
        ChangeHp(MaxHp);
    }

    private void LoadPlayerData(SaveData saveData){
        transform.position = saveData.playerPosition;
    }

    public void ChangeHp(float value){
        // Hp = Mathf.Clamp(Hp + value, 0f, MaxHp);
        // PlayerHUD.Instance.UpdateHpBar();
    }
}