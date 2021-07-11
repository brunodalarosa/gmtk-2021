using UnityEngine;

namespace Block
{
    public class DashBlock : BaseBlock
    {
        [SerializeField]
        private float _dashForce = 2.5f;
        private float DashForce => _dashForce;

        protected override void Action()
        {
            var direction = LeaderBlock.FacingRight ? Vector2.right : Vector2.left;
            LeaderBlock.Rigidbody2D.AddForce(direction * DashForce, ForceMode2D.Impulse);
        }
    }
}
