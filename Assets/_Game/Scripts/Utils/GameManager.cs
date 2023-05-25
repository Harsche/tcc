using Scripts.Camera;
using UnityEngine;

public class GameManager : MonoBehaviour{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private GameCamera _gameCamera;
    public static GameManager Instance{ get; private set; }
    public static PauseMenu PauseMenu{ get; private set; }
    public static GameCamera GameCamera{ get; private set; }

    private static bool GamePaused;


    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SetupStaticFields();
    }

    public static void TogglePause(){
        if (PauseMenu.IsToggling){ return; }
        GamePaused = !GamePaused;
        Time.timeScale = GamePaused ? 0f : 1f;
        GameCamera.ToggleBackgroundBlur(GamePaused);
        PauseMenu.ToggleMenu(GamePaused);
    }

    private void SetupStaticFields(){
        PauseMenu = _pauseMenu;
        GameCamera = _gameCamera;
    }
}