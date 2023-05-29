#if UNITY_EDITOR
using Game.SaveSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils{
    public static class EditorUtils{
        // Menu Items
        
        [MenuItem("Game Utilities/Save/Open Save Folder")]
        private static void OpenSaveFolder(){
            string path = Application.persistentDataPath;
            System.Diagnostics.Process.Start(path);
        }

        [MenuItem("Game Utilities/Save/Delete Save")]
        private static void DeleteSave(){
            SaveSystem.DeleteSaveFile();
        }
        


        // Util Methods

        public static bool IsInPrefabEditorContext(){
            return PrefabStageUtility.GetCurrentPrefabStage() != null;
            // if (){ return false; }
            // PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(selectedObject);
            // return assetType == PrefabAssetType.Regular;
        }
    }
}
#endif