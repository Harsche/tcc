using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game/Game Data", order = 0)]
    public class GameData : ScriptableObject{
        [Label("Elements", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)] [Space(10f)]
        public SerializedDictionary<Element, ElementData> elementsData;

        [Serializable]
        public class ElementData{
            [field: SerializeField] public Color SpriteColor{ get; private set; }
            [field: SerializeField] public Color LightColor{ get; private set; }
            [field: SerializeField, ColorUsage(false, true)] public Color EmissionColor{ get; private set; }
        }

        [Label("Enemies", skinStyle: SkinStyle.Round, Alignment = TextAnchor.MiddleCenter)]
        [Space(10f)]
        [ReorderableList]
        public List<EnemyBase> enemies;
    }
}