using UnityEngine;

namespace GMTK2021
{
    public class JumpBlock : Block
    {
        [SerializeField]
        private float _jumpForce = 15f;
        private float JumpForce => _jumpForce;

        protected override void Action()
        {
            if (PlayerBlock.Grounded)
            {
                PlayerBlock.Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.Jump);
            }
        }
    }
}