using DG.Tweening;
using UnityEngine;

namespace Enemy
{
    public class TriangleController : BaseEnemy
    {
        [SerializeField]
        private bool _goToRight;
        private bool GoToRight => _goToRight;

        [SerializeField]
        private Transform _leftPoint = null;
        private Transform LeftPoint => _leftPoint;

        [SerializeField]
        private Transform _rightPoint = null;
        private Transform RightPoint => _rightPoint;

        [SerializeField]
        private float _speed = 1.5f;
        private float Speed => _speed;

        [SerializeField]
        private Animator _animator;

        private Animator Animator => _animator;

        private void FixedUpdate()
        {
            var direction = GoToRight ? 1 : -1;

            if (GoToRight)
            {
                Animator.SetFloat("MoveX", 1);
            }
            else
            {
                Animator.SetFloat("MoveX", -1);
            }
            
            transform.DOMoveX(transform.position.x + (Speed * 0.01f * direction), 0);

            if (transform.position.x > RightPoint.position.x)
            {
                _goToRight = false;
            }

            if (transform.position.x < LeftPoint.position.x)
            {
                _goToRight = true;
            }
        }
    }
}
