using Cinemachine;
using UnityEngine;

namespace Level
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _cinemachine = null;
        private CinemachineVirtualCamera Cinemachine => _cinemachine;
        
        public static GameplayCamera Instance { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void SetCamera(Transform focusedAgent)
        {
            Cinemachine.Follow = focusedAgent;
            Cinemachine.LookAt = focusedAgent;
        }
    }
}
