using System.Collections;
using UnityEngine;

public class Chest : Interactable{
    [SerializeField] private Rigidbody2D item;
    [SerializeField] private Vector2 minMaxAngle = new(60f, 120f);
    [SerializeField] private float dropForce = 3f;

    private void OpenChest(){
        Rigidbody2D spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
        float angle = Random.Range(minMaxAngle.x, minMaxAngle.y);
        Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        spawnedItem.AddForce(direction * dropForce, ForceMode2D.Impulse);
        Destroy(gameObject, 3f);
    }

    public override void Interact(){
        OpenChest();
        IsInteractable = false;
    }
}