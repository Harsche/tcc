using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.SaveSystem;

public class SaveSlot : MonoBehaviour{
    [SerializeField] private int saveIndex;
    [SerializeField] private GameObject saveInfo;
    [SerializeField] private GameObject newSave;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI locationText;

    private Button button;
    private bool hasSave;

    private void Awake(){
        button = GetComponent<Button>();
        hasSave = SaveSystem.SaveFileExists(saveIndex);
        DisplaySlotInfo();
    }

    public void SelectSaveSlot(){
        SaveSystem.LoadFromFile(saveIndex);
        SceneLoader.Instance.LoadSetupScene();
    }

    private void DisplaySlotInfo(){
        saveInfo.SetActive(hasSave);
        newSave.SetActive(!hasSave);
        if (!hasSave){return; }
        SaveData slotSaveData = SaveSystem.GetSaveData(saveIndex);
        playTimeText.text = slotSaveData.playTime.ToString(@"hh\:mm\:ss");
        locationText.text = slotSaveData.loadScene;
    }
}