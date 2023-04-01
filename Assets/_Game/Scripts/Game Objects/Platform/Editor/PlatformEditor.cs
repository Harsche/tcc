using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameEditors{
    [CustomEditor(typeof(Platform))]
    public class PlatformEditor : Editor{
        [SerializeField] private VisualTreeAsset visualTreeAssetUxml;

        private void OnSceneGUI(){
            var platform = target as Platform;
            if (platform == null){ return; }
            int i = 0;
            if (!Application.isPlaying){
                platform.Waypoints[0] = platform.transform.position;
                i += 1;
            }
            for (int index = i; index < platform.Waypoints.Count; index++){
                Vector3 waypoint = platform.Waypoints[index];
                platform.Waypoints[index] = Handles.PositionHandle(waypoint, Quaternion.identity);
            }
        }

        public override VisualElement CreateInspectorGUI(){
            var platform = target as Platform;
            if (platform == null){ return null; }

            VisualElement inspector = new();

            visualTreeAssetUxml.CloneTree(inspector);

            VisualElement defaultInspector = inspector.Q("Default_Inspector");
            var addButton = inspector.Q<Button>("Add_Button");
            var removeButton = inspector.Q<Button>("Remove_Button");
            addButton.clickable.clickedWithEventInfo += platform.AddWaypoint;
            removeButton.clickable.clickedWithEventInfo += platform.RemoveWaypoint;

            if (defaultInspector != null){
                InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            }
            return inspector;
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
        public static void OnDrawGizmosSelected(Platform platform, GizmoType gizmoType){
            if (!platform.showHandles){ return; }
            List<Vector3> Waypoints = platform.Waypoints;
            if (Waypoints.Count == 0){ return; }

            int lines = platform.PlatformType switch{
                PlatformType.Linear => Waypoints.Count - 1,
                PlatformType.Circular => Waypoints.Count,
                _ => throw new ArgumentOutOfRangeException()
            };

            Vector3 platformPosition = platform.transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(platformPosition, 0.2f);
            Gizmos.color = Color.black;
            if (Application.isPlaying){ Gizmos.DrawSphere(Waypoints[0], 0.2f); }
            for (int index = 0; index < lines; index++){
                Vector3 from = Waypoints[index];
                Vector3 to = index == Waypoints.Count - 1
                    ? Waypoints[0]
                    : Waypoints[index + 1];
                Gizmos.DrawSphere(to, 0.2f);
                Gizmos.DrawLine(from, to);
            }
        }
    }
}