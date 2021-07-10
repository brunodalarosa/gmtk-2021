using System.Collections;
using Manager;
using UnityEngine;

namespace Block
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class WalkingBlock : BaseBlock
    {
        private const float WalkSfxSecondsInterval = 0.4f;
        private const float Smoothness = 0.05f;

        private Rigidbody2D Rigidbody2D { get; set; }

        private Vector3 _velocity = Vector3.zero;

        private bool PlayingWalkSound { get; set; }

        protected override void DidAwake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            PositionFromPlayer = Vector2.zero;
            PlayingWalkSound = false;
        }

        protected override void Action()
        {
            // HandleMovement(value.GetValue());
        }

        public void HandleMovement(float horizontalInput)
        {
            var velocity = Rigidbody2D.velocity;
            Vector3 targetVelocity = new Vector2(horizontalInput, velocity.y);
            Rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, Smoothness);

            if (Rigidbody2D.velocity.x != 0 && !PlayingWalkSound)
            {
                StartCoroutine(PlayWalkSfx());
            }
        }

        private IEnumerator PlayWalkSfx()
        {
            PlayingWalkSound = true;
            yield return new WaitForSeconds(WalkSfxSecondsInterval);
            AudioManager.Instance.PlaySfx(AudioManager.SoundEffects.WalkStep);
            PlayingWalkSound = false;
        }
    }
}