using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour{
    [SerializeField] private int saveIndex;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI locationText;

    private Button button;
    private bool hasSave;

    private void Awake(){
        button = GetComponent<Button>();
        hasSave = GameSaveSystem.SaveFileExists(saveIndex);
    }

    public void SelectSaveSlot(){
        GameSaveSystem.LoadFromFile(saveIndex);
        SceneLoader.Instance.LoadSetupScene();
    }
}