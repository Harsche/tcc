using DG.Tweening;
using UnityEngine;

    public class Item : Interactable{
        public override void Interact(){
            CollectItem();
        }

        private void CollectItem(){
            transform.DOScale(Vector3.zero, 0.5f)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
