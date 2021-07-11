using Manager;
using UnityEngine;

namespace Level
{
    public class Sandbox : MonoBehaviour
    {
        [SerializeField]
        private HeadsUpDisplay _headUpDisplayPrefab = null;
        private HeadsUpDisplay HeadsUpDisplayPrefab => _headUpDisplayPrefab;

        private void Awake()
        {
            if (HeadsUpDisplay.Instance == null) Instantiate(HeadsUpDisplayPrefab);
        }

        private void Start()
        {
            AudioManager.Instance.PlayLevelMusic();
        }
    }
}