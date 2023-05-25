using UnityEngine;
using UnityEngine.SceneManagement;


namespace Data.ScriptableObjects{
    [CreateAssetMenu(fileName = "General Methods", menuName = "General Methods", order = 0)]
    public class GeneralMethods : ScriptableObject{
        public void ChangeScene(string sceneName){
            SceneManager.LoadScene(sceneName);
        }

        public void TogglePause(){
            GameManager.TogglePause();
        }
        
        public void QuitGame(){
            Application.Quit();
        }
    }
}