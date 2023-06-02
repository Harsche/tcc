using Cinemachine;
using UnityEngine;
using Game.SaveSystem;

public class Player : MonoBehaviour{
    [field: SerializeField] public int MaxHp{ get; private set; } = 3;
    [field: SerializeField] public int Hp{ get; private set; }
    [field: SerializeField] public Camera PlayerCamera{ get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera PlayerVirtualCamera{ get; private set; }
    [field: SerializeField] public PlayerAnimation PlayerAnimation{ get; private set; }
    [field: SerializeField] public PlayerMovement PlayerMovement{ get; private set; }
    [field: SerializeField] public ElementMagic ElementMagic{ get; private set; }
    [field: SerializeField] public Parry PlayerParry{ get; private set; }
    [field: SerializeField] public PlayerShield PlayerShield{ get; private set; }
    public static Player Instance{ get; private set; }
    public Interactable Interaction{ get; private set; }

    private Interactable currentInteraction;

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadPlayerData();
    }

    private void Start(){
        ChangeHp(MaxHp);
    }

    private void OnEnable(){
        PlayerInput.OnPlayerInteract += Interact;
    }

    private void OnDisable(){
        PlayerInput.OnPlayerInteract -= Interact;
    }

    private void OnTriggerStay2D(Collider2D col){
        if (col.CompareTag("Interactable")){
            if (Interaction == null){
                var interactable = col.GetComponent<Interactable>();
                if (interactable.IsInteractable){ Interaction = interactable; }
                return;
            }
            Vector3 playerPosition = transform.position;
            float distance = Vector2.Distance(playerPosition, Interaction.transform.position);
            float newDistance = Vector2.Distance(playerPosition, col.transform.position);
            if (newDistance < distance){ Interaction = col.GetComponent<Interactable>(); }
        }
    }

    public void ChangeHp(int value){
        Hp = Mathf.Clamp(Hp + value, 0, MaxHp);
        PlayerHUD.Instance.UpdateHp(Hp);
    }

    private void Interact(){
        if (Interaction == null || !Interaction.IsInteractable){ return; }
        Interaction.Interact();
        // Interaction = null;
    }

    private void LoadPlayerData(){
        SaveData saveData = SaveSystem.SaveData;
        if (saveData.isNewGame){ return; }
        transform.position = saveData.playerPosition;
        PlayerParry.enableParry = saveData.unlockParry;
        PlayerShield.unlocked = saveData.unlockShield;
        PlayerMovement.enableDash = saveData.unlockDash;
    }
}