using UnityEditor;
using UnityEngine;

namespace Scripts.Editor{
    
    public class SaveUtilities : MonoBehaviour{
        [MenuItem("Game Utilities/Save/Delete Save")]
        private static void DeleteSave(){
            GameSaveSystem.DeleteSaveFile();
        }
    }
}