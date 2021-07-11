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

        private void Start()
        {
            Instantiate(HeadsUpDisplayPrefab);

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