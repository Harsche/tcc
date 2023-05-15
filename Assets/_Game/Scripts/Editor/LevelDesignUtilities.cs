using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Editor{
    public static class LevelDesignUtilities{
        [MenuItem("Game Utilities/Level/Organize Props")]
        private static void DeleteSave(){
            List<Transform> props = new();
            Transform propsParent = null;
            foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects()){
                if (rootGameObject.CompareTag("Prop")){
                    props.Add(rootGameObject.transform);
                    continue;
                }
                if (rootGameObject.name == "Props"){ propsParent = rootGameObject.transform; }
            }
            if (!propsParent){ propsParent = new GameObject("Props").transform; }
            foreach (Transform prop in props){ prop.SetParent(propsParent); }
        }
    }
}