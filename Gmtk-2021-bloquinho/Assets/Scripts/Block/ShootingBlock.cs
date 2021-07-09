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
            var bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity, transform);
            bullet.Init(PlayerBlock.FacingRight);
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.Shoot);
        }
    }
}
