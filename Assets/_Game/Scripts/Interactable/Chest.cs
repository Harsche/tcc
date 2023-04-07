using UnityEngine;

public class Chest : Interactable{
    [SerializeField] private Rigidbody2D item;
    [SerializeField] private Vector2 minMaxAngle = new(60f, 120f);

    private void OpenChest(){
        Instantiate(item, transform.position, Quaternion.identity);
        float angle = Random.Range(minMaxAngle.x, minMaxAngle.y);
        Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        item.AddForce(direction, ForceMode2D.Impulse);
    }

    public override void Interact(){
        OpenChest();
    }
}