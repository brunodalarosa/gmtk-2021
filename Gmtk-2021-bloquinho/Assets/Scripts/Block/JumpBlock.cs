using UnityEngine;

namespace GMTK2021
{
    public class JumpBlock : Block
    {
        [SerializeField]
        private float _jumpForce = 1.5f;
        private float JumpForce => _jumpForce;

        protected override void Action()
        {
            if (PlayerBlock.Grounded)
                PlayerBlock.Rigidbody2D.AddForce(Vector2.up * JumpForce);
        }
    }
}