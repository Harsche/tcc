using Scripts.Camera;
using UnityEngine;

public class GameManager : MonoBehaviour{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private GameCamera _gameCamera;
    public static GameManager Instance{ get; private set; }
    public static PauseMenu PauseMenu{ get; private set; }
    public static GameCamera GameCamera{ get; private set; }


    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SetupStaticFields();
    }

    public static void TogglePauseGame(bool value){
        if (PauseMenu.IsToggling){ return; }
        Time.timeScale = value ? 1f : 0f;
        PauseMenu.ToggleMenu(value);
    }

    private void SetupStaticFields(){
        PauseMenu = _pauseMenu;
        GameCamera = _gameCamera;
    }
}