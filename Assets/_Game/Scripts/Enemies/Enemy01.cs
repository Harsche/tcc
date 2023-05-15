using UnityEngine;

public class Enemy01 : MonoBehaviour{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rayCastDistance = 0.75f;

    private Transform myTransform;

    private void Awake(){
        myTransform = transform;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            10f,
            LayerMask.GetMask("Ground")
        );
        transform.position = hit.point;
    }

    private void Update(){
        Vector3 position = myTransform.position;
        Vector3 right = myTransform.right;
        myTransform.Translate(speed * Time.deltaTime * myTransform.TransformVector(right));
        Debug.DrawRay(position, right * rayCastDistance, Color.black);
        RaycastHit2D hit = Physics2D.Raycast(
            position,
            right,
            rayCastDistance,
            LayerMask.GetMask("Ground")
        );
        if (hit.collider){ myTransform.Rotate(Vector3.up, 180f); }
    }
}