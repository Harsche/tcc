using System;
using UnityEngine;

public class EnergyLine : MonoBehaviour{
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private EnemyAttackDirection enemyAttackDirection;
    [SerializeField] private MagicType energyType;
    [SerializeField] private Gradient gradientRed;
    [SerializeField] private Gradient gradientBlue;
    [SerializeField] private Gradient gradientGreen;

    private readonly RaycastHit2D[] hit = new RaycastHit2D[1];
    private LineRenderer lineRenderer;

    private void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.colorGradient = energyType switch{
            MagicType.Red => gradientRed,
            MagicType.Green => gradientGreen,
            MagicType.Blue => gradientBlue,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void Update(){
        Vector2 origin = transform.position;
        Quaternion directionRotation = Quaternion.AngleAxis(90f * (int) enemyAttackDirection, Vector3.forward);
        Vector2 direction = directionRotation * Vector2.right;
        if (Physics2D.RaycastNonAlloc(origin, direction, hit, maxDistance) < 1){
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, direction * maxDistance);
            return;
        }

        if (!hit[0].collider.CompareTag("PlayerShield")){
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit[0].point));
            return;
        }
        
        ReflectLine();

    }

    private void ReflectLine(){
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit[0].point));
        float distanceLeft = maxDistance - Vector2.Distance(transform.position, hit[0].point);
        Vector2 finalPosition = hit[0].point + PlayerInput.LookDirection * distanceLeft;
        // Vector2 finalPosition = hit[0].point + hit[0].normal * distanceLeft;
        if (Physics2D.RaycastNonAlloc(hit[0].point, PlayerInput.LookDirection, hit, distanceLeft) < 1){
            lineRenderer.SetPosition(2, PlayerInput.LookDirection * distanceLeft);
            return;
        }
        lineRenderer.SetPosition(2, transform.InverseTransformPoint(hit[0].point));
        if(!hit[0].collider.CompareTag("Button")){return;}
        hit[0].collider.GetComponent<GameButton>().Interact(MagicType.Blue);
    }
}