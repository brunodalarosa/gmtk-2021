using UnityEngine;

namespace Block
{
    public class JumpBlock : Block
    {
        [SerializeField]
        private float _jumpForce = 15f;
        public float JumpForce => _jumpForce;

        protected override void Action()
        {
        }
    }
}