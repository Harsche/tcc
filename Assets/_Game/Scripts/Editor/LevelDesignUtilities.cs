using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.Editor{
    public static class LevelDesignUtilities{
        private const string OrganizeProps = "Game Utilities/Level/Organize Props";
        private const string SnapCharactersToGroundPath = "Game Utilities/Level/Snap Characters To Ground";
        private static bool SnapCharactersToGround;
        

        [MenuItem(OrganizeProps)]
        private static void DeleteSave(){
            List<Transform> props = new();
            Transform propsParent = null;
            foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects()){
                if (rootGameObject.CompareTag("Prop")){
                    props.Add(rootGameObject.transform);
                    continue;
                }
                if (rootGameObject.name == "=====Props====="){ propsParent = rootGameObject.transform; }
            }
            if (!propsParent){ propsParent = new GameObject("=====Props=====").transform; }
            foreach (Transform prop in props){ prop.SetParent(propsParent); }
        }

        [MenuItem(SnapCharactersToGroundPath)]
        private static void ToggleSnapCharactersToGround(){
            Menu.SetChecked(SnapCharactersToGroundPath, !SnapCharactersToGround);
        }
    }
}