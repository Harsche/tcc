using UnityEditor;
using UnityEngine;

namespace GameEditors{
    [CustomEditor(typeof(SceneTransition))]
    public class SceneTransitionEditor : Editor{
        private void OnSceneGUI(){
            var sceneTransition = target as SceneTransition;
            if (sceneTransition == null || Application.isPlaying){ return; }
            sceneTransition.Destination =
                Handles.PositionHandle(sceneTransition.Destination, Quaternion.identity);
            EditorUtility.SetDirty(sceneTransition);
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
        public static void OnDrawGizmosSelected(SceneTransition sceneTransition, GizmoType gizmoType){
            Gizmos.color = Color.green;
            Gizmos.DrawCube(sceneTransition.Destination, new Vector3(0.72f, 1.74f, 0.1f));
        }
    }
}