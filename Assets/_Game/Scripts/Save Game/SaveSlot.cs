using Game.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour{
    [SerializeField] private int saveIndex;
    [SerializeField] private GameObject saveInfo;
    [SerializeField] private GameObject newSave;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private TextMeshProUGUI panelSlotText;
    [SerializeField] private ConfirmAction confirmAction;
    [SerializeField] private GameObject deleteButton;

    private Button button;
    private bool hasSave;

    private void Awake(){
        button = GetComponent<Button>();
        DisplaySlotInfo();
    }

#if UNITY_EDITOR
    private void OnValidate(){
        panelSlotText.text = $"Save {saveIndex + 1}";
    }
#endif

    public void DeleteSaveSlot(){
        confirmAction.gameObject.SetActive(true);
        confirmAction.OnConfirm.AddListener(DeleteSaveAndUpdateSlot);
        confirmAction.OnCancel.AddListener(CancelDeletion);

        void DeleteSaveAndUpdateSlot(){
            SaveSystem.DeleteSaveFile(saveIndex);
            DisplaySlotInfo();
        }

        void CancelDeletion(){
            confirmAction.OnConfirm.RemoveListener(DeleteSaveAndUpdateSlot);
        }
    }

    public void SelectSaveSlot(){
        if (hasSave){ SaveSystem.LoadFromFile(saveIndex); }
        else{ SaveSystem.StartNewGame(saveIndex); }
        SceneLoader.Instance.LoadSetupScene();
    }

    private void DisplaySlotInfo(){
        hasSave = SaveSystem.SaveFileExists(saveIndex);
        deleteButton.SetActive(hasSave);
        saveInfo.SetActive(hasSave);
        newSave.SetActive(!hasSave);
        if (!hasSave){ return; }
        SaveData slotSaveData = SaveSystem.GetSaveData(saveIndex);
        playTimeText.text = slotSaveData.playTime.ToString(@"hh\:mm\:ss");
        locationText.text = slotSaveData.loadScene;
    }
}