using System;
using UnityEngine;

namespace GMTK2021
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _backgroundMusicTrack;
        protected AudioSource MainMusicTrack
        {
            get => _backgroundMusicTrack;
            set => _backgroundMusicTrack = value;
        }
        
        protected AudioSource SfxTrack { get; set; }
        
        public AudioClip _mainMenuMusic;
        public AudioClip _backgroundMusic;
        
        public AudioClip _blockConnectSfx;
        public AudioClip _walkStepSfx;
        public AudioClip _shootSfx;
        public AudioClip _jumpSfx;
        public AudioClip _enemyPoofSfx;
        public AudioClip _levelCompleteSfx;
        public AudioClip _playFirstLevelSfx;

        private void Awake()
        {
            MainMusicTrack = gameObject.AddComponent<AudioSource>();
            MainMusicTrack.loop = true;
            MainMusicTrack.volume = 0.7f;

            SfxTrack = gameObject.AddComponent<AudioSource>();
            SfxTrack.loop = false;
        }

        public void PlayMainMenuMusic()
        {
            PlayBGM(_mainMenuMusic);
        }

        public void PlayLevelMusic()
        {
            PlayBGM(_backgroundMusic);
        }

        private void PlayBGM(AudioClip bgm)
        {
            if (MainMusicTrack.clip == bgm) return;

            MainMusicTrack.Stop();

            MainMusicTrack.clip = bgm;

            MainMusicTrack.Play();
        }
        
        public void PlaySfx(SoundEffects sfx)
        {
            PlaySfx(GetSfxAudioClip(sfx));
        }

        private AudioClip GetSfxAudioClip(SoundEffects sfx)
        {
            switch (sfx)
            {
                case SoundEffects.Unknown:
                    return null;
                case SoundEffects.BlockConnect:
                    return _blockConnectSfx;
                case SoundEffects.WalkStep:
                    return _walkStepSfx;
                case SoundEffects.Shoot:
                    return _shootSfx;
                case SoundEffects.Jump:
                    return _jumpSfx;
                case SoundEffects.EnemyPoof:
                    return _enemyPoofSfx;
                case SoundEffects.LevelComplete:
                    return _levelCompleteSfx;
                case SoundEffects.PlayFirstLevel:
                    return _playFirstLevelSfx;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sfx), sfx, null);
            }
        }

        private void PlaySfx(AudioClip sfx)
        {
            if (sfx == null) return;

            PlaySound(SfxTrack, sfx);
        }
        
        private void PlaySound(AudioSource source, AudioClip sound)
        {
            if (!source) throw new InvalidOperationException("AudioSource can't be null!");

            if (!sound) throw new InvalidOperationException("AudioClip can't be null!");

            source.PlayOneShot(sound, 1);
        }
        
        private void OnDestroy()
        {
            MainMusicTrack.Stop();
            SfxTrack.Stop();
        }

        public enum SoundEffects
        {
            Unknown = 0,
            BlockConnect = 1,
            WalkStep = 2,
            Shoot = 3,
            Jump = 4,
            EnemyPoof = 5,
            LevelComplete = 6,
            PlayFirstLevel = 7
        }
    }
}