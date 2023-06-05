using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Scripts.Enemies.Editor{
    [CustomEditor(typeof(EnemyBase))]
    public class EnemyBaseEditor : UnityEditor.Editor{
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
        public static void OnDrawGizmosSelected(EnemyBase enemy, GizmoType gizmoType){
            Handles.color = Color.red;
            Handles.DrawWireDisc(enemy.transform.position, Vector3.back, enemy.MaxPlayerDistance);
            Handles.color = Color.red * new Vector4(1f, 1f, 1f, 0.05f);
            Handles.DrawSolidDisc(enemy.transform.position, Vector3.back, enemy.MaxPlayerDistance);
        }
    }
}