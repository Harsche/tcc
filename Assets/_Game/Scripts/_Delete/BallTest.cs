using System;
using UnityEngine;

namespace Scripts._Delete{
    public class BallTest : MonoBehaviour{
        private Rigidbody2D _rigidbody2D;
        
        private void Awake(){
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate(){
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.y = 0f;
            _rigidbody2D.velocity = velocity;
        }
    }
}