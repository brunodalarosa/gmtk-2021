using UnityEngine;

namespace Block
{
    public class JumpBlock : BaseBlock
    {
        [SerializeField]
        private float _jumpForce = 15f;
        public float JumpForce => _jumpForce;
        
        protected override void Action()
        {
           //todo
        }
    }
}