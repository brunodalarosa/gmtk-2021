using UnityEngine;

namespace GMTK2021
{
    public class JumpBlock : Block
    {
        protected override void Action()
        {
            PlayerBlock.Rigidbody2D.AddForce(Vector2.up * 3f);
        }
    }
}