using Manager;
using UnityEngine;

namespace Block
{
    public class ShootingBlock : BaseBlock
    {
        [SerializeField]
        private BulletController _bulletPrefab = null;
        private BulletController BulletPrefab => _bulletPrefab;

        protected override void Action()
        {
            var instantiantionPosition =
                transform.position + (LeaderBlock.FacingRight ? Vector3.right * 0.75f : Vector3.left * 0.75f);
            
            var bullet = Instantiate(BulletPrefab, instantiantionPosition, Quaternion.identity);
            bullet.Init(LeaderBlock.FacingRight);
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.Shoot);
        }
    }
}
