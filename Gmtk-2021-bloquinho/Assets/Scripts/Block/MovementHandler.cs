using UnityEngine;

namespace Block
{
    public class MovementHandler : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D;

        private Vector3 Velocity = Vector3.zero;
        private float Smoothness = 0.05f;
        
        void Update()
        {
            Vector3 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10f, Rigidbody2D.velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, Smoothness);
        }
    }
}
