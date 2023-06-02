using UnityEngine;
using UnityEngine.SceneManagement;
using Game.SaveSystem;


namespace Data.ScriptableObjects{
    [CreateAssetMenu(fileName = "General Methods", menuName = "Game/General Methods", order = 1)]
    public class GeneralMethods : ScriptableObject{
        public void UnlockParry(){
            Player.Instance.PlayerParry.enableParry = true;
            SaveSystem.SaveData.unlockParry = true;
        }
        
        public void UnlockDash(){
            Player.Instance.PlayerMovement.enableDash = true;
            SaveSystem.SaveData.unlockDash = true;
        }
        
        public void ChangeScene(string sceneName){
            SceneManager.LoadScene(sceneName);
        }
        
        public void LoadScene(string sceneName){
            SceneLoader.Instance.LoadScene(sceneName);
        }

        public void TogglePause(){
            GameManager.TogglePause();
        }
        
        public void QuitGame(){
            Application.Quit();
        }
    }
}