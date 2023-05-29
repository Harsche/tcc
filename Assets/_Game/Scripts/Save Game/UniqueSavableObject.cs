using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.SaveSystem{
    [RequireComponent(typeof(GuidComponent))]
    public class UniqueSavableObject : MonoBehaviour, ISavable{
        [FormerlySerializedAs("saveId")] [SerializeField] [Disable] [NewLabel("GUID")]
        private string saveKey;

        [SerializeField] private bool saveActive;
        [SerializeField] private bool savePosition;

        private GuidComponent guidComponent;

        private void Start(){
            (this as ISavable).LoadState();
        }

        private void OnDestroy(){
            (this as ISavable).SaveState();
        }

#if UNITY_EDITOR
        private void OnValidate(){
            if (guidComponent){ return; }
            guidComponent = GetComponent<GuidComponent>();
            saveKey = guidComponent.GetGuid().ToString();
        }
#endif

        void ISavable.SaveState(){
            Dictionary<string, object> data = new();
            if (saveActive){ data.Add("active", gameObject.activeSelf); }
            if (savePosition){ data.Add("position", transform.localPosition); }
            SaveSystem.SaveData.AddUniqueObjectData(saveKey, data);
        }

        void ISavable.LoadState(){
            if (!SaveSystem.SaveData.TryGetUniqueObjectData(saveKey, out Dictionary<string, object> data)){ return; }
            if (saveActive){ gameObject.SetActive((bool) data["active"]); }
            if (savePosition){ transform.localPosition =  data["position"]; }
        }

        public void SaveObject(){
            (this as ISavable).SaveState();
        }
    }
}