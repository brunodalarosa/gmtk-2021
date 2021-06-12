using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriangleController : MonoBehaviour
{
    [SerializeField]
    private bool _goToRight;
    private bool GoToRight => _goToRight;

    [SerializeField]
    private Transform _pointA = null;
    private Transform PointA => _pointA;

    public GameObject PointB;

    [SerializeField]
    private float _speed = 1.5f;
    private float Speed => _speed;

    private void Start()
    {
        var coisa = PointB.transform;
        // var direction = GoToRight ? 1 : -1;
        // transform.DOMoveX(transform.position.x + direction, Speed);

        // if (transform.position.x >= PointB.transform.position.x)
        //     _goToRight = !GoToRight;
        // else if (transform.position.x <= PointA.transform.position.x)
        //     _goToRight = !GoToRight;
    }
}
