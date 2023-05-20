using UnityEngine;

namespace Scripts.Enemies{
    public class EnemyAnimation : MonoBehaviour{
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D myRigidbody2d;
        [SerializeField] private bool isSpriteFlipped;

        private void Update(){
            float xVelocity = myRigidbody2d.velocity.x;
            if (xVelocity != 0){
                bool flipX = xVelocity < 0;
                if (isSpriteFlipped){ flipX = !flipX; }
                spriteRenderer.flipX = flipX;
            }
        }
    }
}