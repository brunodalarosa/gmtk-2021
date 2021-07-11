using UnityEngine;

namespace Block
{
    public class MovementHandler : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D;

        private Vector3 _velocity = Vector3.zero;
        private const float Smoothness = 0.05f;

        private void Update()
        {
            var velocity = Rigidbody2D.velocity;
            Vector3 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * 10f, velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, Smoothness);
        }
    }
}
