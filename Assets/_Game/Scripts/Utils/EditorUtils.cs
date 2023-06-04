#if UNITY_EDITOR
using System.Diagnostics;
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
            Process.Start(path);
        }

        [MenuItem("Game Utilities/Save/Delete Save")]
        private static void DeleteSave(){
            for (int i = 0; i < 3; i++)
                SaveSystem.DeleteSaveFile(i);
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