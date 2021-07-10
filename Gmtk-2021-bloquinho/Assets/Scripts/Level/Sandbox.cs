using Manager;
using UnityEngine;

namespace Level
{
    public class Sandbox : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.PlayLevelMusic();
        }
    }
}