using System.Collections;
using System.Collections.Generic;
using GMTK2021;
using UnityEngine;

public class LagolasBlock : Block
{
    [SerializeField]
    private BulletController _bulletPrefab = null;
    private BulletController BulletPrefab => _bulletPrefab;

    protected override void Action()
    {
        var bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity, transform);
        bullet.Init(PlayerBlock.FacingRight);
    }
}
