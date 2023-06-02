using System;
using UnityEngine;

namespace Data.ScriptableObjects{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game/Game Data", order = 0)]
    public class GameData : ScriptableObject{
        [Label("Elements Data", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)]
        public SerializedDictionary<Element, ElementData> elementsData;
            
        [Serializable]
        public class ElementData{
            [field: SerializeField] public Color SpriteColor{ get; private set; }
            [field: SerializeField] public Color LightColor{ get; private set; }
            [field: SerializeField, ColorUsage(false, true)] public Color EmissionColor{ get; private set; }
        }
    }
    
    
}