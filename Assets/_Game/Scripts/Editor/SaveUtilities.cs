using UnityEditor;
using UnityEngine;
using Game.SaveSystem;

namespace Scripts.Editor{
    
    public static class SaveUtilities{
        [MenuItem("Game Utilities/Save/Delete Save")]
        private static void DeleteSave(){
            SaveSystem.DeleteSaveFile();
        }
    }
}