using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Editors{
    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementEditor : Editor{
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        public override VisualElement CreateInspectorGUI(){
            VisualElement inspector = new();
            visualTreeAsset.CloneTree(inspector);
            VisualElement defaultInspector = inspector.Q("Default_Inspector");
            if (defaultInspector != null){
                InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            }
            return inspector;
        }
    }
}