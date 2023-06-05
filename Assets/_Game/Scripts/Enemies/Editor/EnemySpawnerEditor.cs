using UnityEditor;
using UnityEngine;

namespace Scripts.Enemies.Editor{
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : UnityEditor.Editor{
        public override void OnInspectorGUI(){

            DrawDefaultInspector();
            var enemySpawner = (EnemySpawner) target;
            if (GUILayout.Button("Spawn Enemy")){
                Instantiate(enemySpawner.EnemyPrefab, enemySpawner.transform);
            }
        }
    }
}