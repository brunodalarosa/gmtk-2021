using Block;
using UnityEngine;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private PlayerBlock _player = null;
        private PlayerBlock Player => _player;

        private void Start()
        {
            GameplayCamera.Instance.SetCamera(Player.transform);
        }
    }
}