using DG.Tweening;
using UnityEngine;

    public class Item : Interactable{
        public override void Interact(){
            CollectItem();
            gameObject.SetActive(false);
        }

        private void CollectItem(){
            Transform parent = transform.parent;
            parent.DOScale(Vector3.zero, 0.5f)
                .OnComplete(() => parent.gameObject.SetActive(false));
        }
    }
