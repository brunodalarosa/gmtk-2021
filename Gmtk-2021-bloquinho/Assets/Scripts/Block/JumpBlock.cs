using System.Linq;
using UnityEngine;

namespace GMTK2021
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