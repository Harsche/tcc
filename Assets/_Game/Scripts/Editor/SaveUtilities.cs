using UnityEditor;
using UnityEngine;

namespace Scripts.Editor{
    
    public static class SaveUtilities{
        [MenuItem("Game Utilities/Save/Delete Save")]
        private static void DeleteSave(){
            GameSaveSystem.DeleteSaveFile();
        }
    }
}