using Manager;
using UnityEngine;

namespace Level
{
    public class Sandbox : MonoBehaviour
    {
        [SerializeField]
        private HeadsUpDisplay _headUpDisplayPrefab = null;
        private HeadsUpDisplay HeadsUpDisplayPrefab => _headUpDisplayPrefab;

        [SerializeField]
        private GameplayCamera _gameplayCameraPrefab = null;
        private GameplayCamera GameplayCameraPrefab => _gameplayCameraPrefab;

        private void Awake()
        {
            if (HeadsUpDisplay.Instance == null) Instantiate(HeadsUpDisplayPrefab);
            if (GameplayCamera.Instance == null) Instantiate(GameplayCameraPrefab);
        }

        private void Start()
        {
            AudioManager.Instance.PlayLevelMusic();
        }
    }
}