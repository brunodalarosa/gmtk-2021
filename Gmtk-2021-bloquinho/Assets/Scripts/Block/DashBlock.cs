using System.Collections;
using System.Collections.Generic;
using GMTK2021;
using UnityEngine;

public class DashBlock : Block
{
    [SerializeField]
    private float _dashForce = 2.5f;
    private float DashForce => _dashForce;

    protected override void Action()
    {
        var direction = PlayerBlock.FacingRight ? 1 : -1;

        PlayerBlock.Rigidbody2D.AddForce(new Vector2(direction, 0) * DashForce);
    }
}
