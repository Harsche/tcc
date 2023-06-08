using Game.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeGame : MonoBehaviour{
    [SerializeField] private SerializedScene startScreenScene;
    
    private void Awake(){
        SaveSystem.SetupSaveSystem();
        SceneManager.LoadScene(startScreenScene.BuildIndex);
    }
}