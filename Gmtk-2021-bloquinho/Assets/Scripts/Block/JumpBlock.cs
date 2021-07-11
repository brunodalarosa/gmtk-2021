using Manager;
using UnityEngine;

namespace Block
{
    public class JumpBlock : BaseBlock
    {
        public static float JumpForce => 21f;
        
        protected override void Action()
        {
            if (LeaderBlock.Grounded)
            {
                LeaderBlock.Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.Jump);
            }
        }
    }
}