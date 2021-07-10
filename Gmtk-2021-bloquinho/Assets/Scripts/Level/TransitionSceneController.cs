using System.Collections;
using Manager;
using UnityEngine;

namespace Level
{
    public class TransitionSceneController : MonoBehaviour
    {
        [SerializeField]
        private HeadsUpDisplay _headUpDisplayPrefab = null;
        private HeadsUpDisplay HeadsUpDisplayPrefab => _headUpDisplayPrefab;

        [SerializeField]
        private GameplayCamera _gameplayCameraPrefab = null;
        private GameplayCamera GameplayCameraPrefab => _gameplayCameraPrefab;

        private void Start()
        {
            Instantiate(HeadsUpDisplayPrefab);
            Instantiate(GameplayCameraPrefab);

            StartCoroutine(TransitionToGameplayCoroutine());
        }

        private IEnumerator TransitionToGameplayCoroutine()
        {
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlayLevelMusic();
            AdventureModeManager.Instance.GoToNextLevel();
        }
    }
}