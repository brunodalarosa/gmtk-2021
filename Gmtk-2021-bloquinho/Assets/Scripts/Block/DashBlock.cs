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
            var direction = LeaderBlock.FacingRight ? 1 : -1;

            LeaderBlock.Rigidbody2D.AddForce(new Vector2(direction, 0) * DashForce, ForceMode2D.Impulse);
        }
    }
}
